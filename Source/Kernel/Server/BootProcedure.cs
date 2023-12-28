// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Diagnostics;
using Aksio.Cratis.Boot;
using Aksio.Cratis.EventSequences.Inboxes;
using Aksio.Cratis.Identities;
using Aksio.Cratis.Kernel.Configuration;
using Aksio.Cratis.Kernel.Grains.EventSequences;
using Aksio.Cratis.Kernel.Grains.EventSequences.Inbox;
using Aksio.Cratis.Kernel.Grains.EventSequences.Streaming;
using Aksio.Cratis.Kernel.Grains.Jobs;
using Aksio.Cratis.Kernel.Grains.Projections;

namespace Aksio.Cratis.Kernel.Server;

/// <summary>
/// Represents a <see cref="IPerformBootProcedure"/> for the event store.
/// </summary>
public class BootProcedure : IPerformBootProcedure
{
    readonly IServiceProvider _serviceProvider;
    readonly IExecutionContextManager _executionContextManager;
    readonly IGrainFactory _grainFactory;
    readonly KernelConfiguration _configuration;
    readonly ILogger<BootProcedure> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="BootProcedure"/> class.
    /// </summary>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> for getting services.</param>
    /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
    /// <param name="grainFactory"><see cref="IGrainFactory"/> for getting grains.</param>
    /// <param name="configuration">The <see cref="KernelConfiguration"/>.</param>
    /// <param name="logger">Logger for logging.</param>
    public BootProcedure(
        IServiceProvider serviceProvider,
        IExecutionContextManager executionContextManager,
        IGrainFactory grainFactory,
        KernelConfiguration configuration,
        ILogger<BootProcedure> logger)
    {
        _serviceProvider = serviceProvider;
        _executionContextManager = executionContextManager;
        _grainFactory = grainFactory;
        _configuration = configuration;
        _logger = logger;
    }

    /// <inheritdoc/>
    public void Perform()
    {
        Task.Run(async () =>
        {
            foreach (var (microserviceId, microservice) in _configuration.Microservices)
            {
                _executionContextManager.Establish(microserviceId);

                _logger.PopulateSchemaStore();
                var schemaStore = _serviceProvider.GetRequiredService<Schemas.ISchemaStore>()!;
                await schemaStore.Populate();

                _logger.PopulateIdentityStore();
                var identityStore = _serviceProvider.GetRequiredService<IIdentityStore>()!;
                await identityStore.Populate();

                foreach (var (tenantId, _) in _configuration.Tenants)
                {
                    _executionContextManager.Establish(tenantId, CorrelationId.New(), microserviceId);

                    foreach (var outbox in microservice.Inbox.FromOutboxes)
                    {
                        var key = new InboxKey(tenantId, outbox.Microservice);
                        var inbox = _grainFactory.GetGrain<IInbox>((MicroserviceId)microserviceId, key);
                        await inbox.Start();
                    }

                    _logger.RehydratingEventSequences(microserviceId, tenantId);
                    var stopwatch = Stopwatch.StartNew();
                    await _grainFactory.GetGrain<IEventSequences>(0, new EventSequencesKey(microserviceId, tenantId)).Rehydrate();
                    stopwatch.Stop();
                    _logger.RehydratedEventSequences(microserviceId, tenantId, stopwatch.Elapsed);

                    _logger.RehydrateJobs(microserviceId, tenantId);
                    await _grainFactory.GetGrain<IJobsManager>(0, new JobsManagerKey(microserviceId, tenantId)).Rehydrate();

                    _logger.RehydrateObservers(microserviceId, tenantId);
                    await _grainFactory.GetGrain<IObservers>(0, new ObserversKey(microserviceId, tenantId)).Rehydrate();
                }
            }

            _logger.RehydrateProjections();
            var projections = _grainFactory.GetGrain<IProjections>(0);
            await projections.Rehydrate();

            _logger.PrimingEventSequenceCaches();
            var eventSequenceCaches = _serviceProvider.GetRequiredService<IEventSequenceCaches>()!;
            await eventSequenceCaches.PrimeAll();
        }).Wait();
    }
}
