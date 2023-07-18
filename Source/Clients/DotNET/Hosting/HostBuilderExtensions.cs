// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis;
using Microsoft.Extensions.Logging;

namespace Microsoft.Extensions.Hosting;

/// <summary>
/// Extensions for using Aksio.Cratis in an application.
/// </summary>
public static class HostBuilderExtensions
{
    /// <summary>
    /// Configures the <see cref="IClientBuilder"/> for a non-microservice oriented scenario.
    /// </summary>
    /// <param name="hostBuilder"><see cref="IHostBuilder"/> to build on.</param>
    /// <param name="configureDelegate">Optional delegate used to configure the Cratis client.</param>
    /// <param name="loggerFactory">Optional <see cref="ILoggerFactory"/>.</param>
    /// <returns><see cref="IHostBuilder"/> for configuration continuation.</returns>
    public static IHostBuilder UseCratis(
        this IHostBuilder hostBuilder,
        Action<IClientBuilder>? configureDelegate = default,
        ILoggerFactory? loggerFactory = default)
    {
        loggerFactory ??= LoggerFactory.Create(builder => builder.AddConsole());
        hostBuilder.ConfigureServices((context, services) =>
        {
            var clientBuilder = new ClientBuilder(services, loggerFactory.CreateLogger<ClientBuilder>());
            configureDelegate?.Invoke(clientBuilder);
            clientBuilder.Build();
        });
        return hostBuilder;
    }
}
