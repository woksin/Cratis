// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Aksio.Configuration;
using Aksio.Cratis.Net;
using Aksio.Tasks;
using Aksio.Timers;
using Azure.Data.Tables;
using Microsoft.AspNetCore.Hosting.Server;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Aksio.Cratis.Clients;

/// <summary>
/// Represents a <see cref="ClusteredKernelClient"/> for Orleans Azure Table store cluster info.
/// </summary>
public class OrleansAzureTableStoreKernelClient : ClusteredKernelClient
{
    readonly IOptions<ClientConfiguration> _options;
    readonly ILogger<OrleansAzureTableStoreKernelClient> _clientLogger;
    IEnumerable<Uri> _endpoints = Enumerable.Empty<Uri>();

    /// <summary>
    /// Initializes a new instance of the <see cref="ClusteredKernelClient"/> class.
    /// </summary>
    /// <param name="options">The <see cref="ClientConfiguration"/>.</param>
    /// <param name="server">The ASP.NET Core <see cref="IServer"/>.</param>
    /// <param name="httpClientFactory">The <see cref="ILoadBalancedHttpClientFactory"/> to use.</param>
    /// <param name="taskFactory">A <see cref="ITaskFactory"/> for creating tasks.</param>
    /// <param name="timerFactory">A <see cref="ITimerFactory"/> for creating timers.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
    /// <param name="clientLifecycle"><see cref="IConnectionLifecycle"/> for communicating lifecycle events outside.</param>
    /// <param name="jsonSerializerOptions"><see cref="JsonSerializerOptions"/> for serialization.</param>
    /// <param name="clientLogger">The <see cref="ILogger"/> for this client.</param>
    /// <param name="logger"><see cref="ILogger"/> for logging.</param>
    public OrleansAzureTableStoreKernelClient(
        IOptions<ClientConfiguration> options,
        IServer server,
        ILoadBalancedHttpClientFactory httpClientFactory,
        ITaskFactory taskFactory,
        ITimerFactory timerFactory,
        IExecutionContextManager executionContextManager,
        IConnectionLifecycle clientLifecycle,
        JsonSerializerOptions jsonSerializerOptions,
        ILogger<OrleansAzureTableStoreKernelClient> clientLogger,
        ILogger<RestKernelClient> logger) : base(
            options,
            server,
            httpClientFactory,
            taskFactory,
            timerFactory,
            executionContextManager,
            clientLifecycle,
            jsonSerializerOptions,
            logger)
    {
        _options = options;
        _clientLogger = clientLogger;
        RefreshSilos();
        timerFactory.Create(_ => RefreshSilos(), 30000, 30000);
    }

    /// <inheritdoc/>
    protected override IEnumerable<Uri> Endpoints => _endpoints;

    /// <inheritdoc/>
    protected override Task OnDisconnected()
    {
        RefreshSilos();
        return Task.CompletedTask;
    }

    /// <inheritdoc/>
    protected override Task OnKernelUnavailable()
    {
        RefreshSilos();
        return Task.CompletedTask;
    }

    void RefreshSilos()
    {
        var options = _options.Value.Kernel.AzureStorageClusterOptions!;
        _clientLogger.GettingSilosFromStorage();
        var client = new TableClient(
            options.ConnectionString,
            options.TableName);

        var result = client.Query<OrleansSiloInfo>(filter: "Status eq 'Active'");
        _endpoints = result.Select(_ => new Uri($"{(options.Secure ? "https" : "http")}://{_.Address}:{options.Port}")).ToArray().AsEnumerable();

        _clientLogger.UsingEndpoints(string.Join(", ", _endpoints));
    }
}
