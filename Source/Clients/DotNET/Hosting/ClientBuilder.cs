// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Collections;
using Aksio.Cratis.Compliance;
using Aksio.Cratis.Connections;
using Aksio.Cratis.Events;
using Aksio.Cratis.Events.Observation;
using Aksio.Cratis.Events.Projections;
using Aksio.Cratis.Events.Schemas;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Extensions.MongoDB;
using Aksio.Cratis.Extensions.Orleans.Execution;
using Aksio.Cratis.Schemas;
using Aksio.Cratis.Types;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Orleans;
using Orleans.Hosting;
using HostBuilderContext = Microsoft.Extensions.Hosting.HostBuilderContext;
using OrleansClientBuilder = Orleans.ClientBuilder;

namespace Aksio.Cratis.Hosting
{
    /// <summary>
    /// Represents an implementation of <see cref="IClientBuilder"/>.
    /// </summary>
    public class ClientBuilder : IClientBuilder
    {
        #pragma warning disable IDE0052 // We will be expanding on this.
        readonly MicroserviceId _microserviceId;

        bool _inSilo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientBuilder"/> class.
        /// </summary>
        /// <param name="microserviceId">Microservice identifier.</param>
        public ClientBuilder(MicroserviceId microserviceId)
        {
            _microserviceId = microserviceId;
        }

        /// <summary>
        /// Start configuring <see cref="IClientBuilder"/> for a specific <see cref="MicroserviceId"/>.
        /// </summary>
        /// <param name="id"><see cref="MicroserviceId"/>.</param>
        /// <returns><see cref="IClientBuilder"/> to build.</returns>
        public static IClientBuilder ForMicroservice(MicroserviceId id)
        {
            return new ClientBuilder(id);
        }

        /// <inheritdoc/>
        public IClientBuilder InSilo()
        {
            _inSilo = true;
            return this;
        }

        /// <inheritdoc/>
        public void Build(HostBuilderContext hostBuilderContext, IServiceCollection services, ITypes? types = default)
        {
            if (types == default)
            {
                types = new Types.Types();
            }

            services
                .AddMongoDBReadModels(types)
                .AddTransient(sp => sp.GetService<IEventStore>()!.EventLog);

            if (_inSilo)
            {
                return;
            }

            var connectionManager = new ConnectionManager();
            services
                .AddSingleton<IConnectionManager>(connectionManager)
                .AddTransient(typeof(IInstancesOf<>), typeof(InstancesOf<>))
                .AddTransient(typeof(IImplementationsOf<>), typeof(ImplementationsOf<>))
                .AddTransient<IEventStore, EventStore>()
                .AddSingleton(types)
                .AddSingleton<IProjectionsRegistrar, ProjectionsRegistrar>()
                .AddProjections()
                .AddSingleton<IObservers, Observers>()
                .AddSingleton<IComplianceMetadataResolver, ComplianceMetadataResolver>()
                .AddSingleton<IJsonSchemaGenerator, JsonSchemaGenerator>()
                .AddSingleton<ISchemas, Events.Schemas.Schemas>()
                .AddSingleton<IEventTypes, EventTypes>()
                .AddSingleton<IEventSerializer, EventSerializer>()
                .AddSingleton<IHostedService, ObserversService>()
                .AddSingleton<IExecutionContextManager, ExecutionContextManager>()
                .AddSingleton<IMongoDBClientFactory, MongoDBClientFactory>()
                .AddSingleton<IRequestContextManager, RequestContextManager>();
            types.AllObservers().ForEach(_ => services.AddTransient(_));

            types.All.Where(_ =>
                _ != typeof(ICanProvideComplianceMetadataForType) &&
                _.IsAssignableTo(typeof(ICanProvideComplianceMetadataForType))).ForEach(_ => services.AddTransient(_));
            types.All.Where(_ =>
                _ != typeof(ICanProvideComplianceMetadataForProperty) &&
                _.IsAssignableTo(typeof(ICanProvideComplianceMetadataForProperty))).ForEach(_ => services.AddTransient(_));

            var orleansBuilder = new OrleansClientBuilder()
                .UseLocalhostClustering()
                .AddEventLogStream()
                .AddSimpleMessageStreamProvider("observer-handlers")
                .UseExecutionContext()
                .AddOutgoingGrainCallFilter<ConnectionIdOutputCallFilter>()
                .ConfigureServices(services => services
                    .AddSingleton<IConnectionManager>(connectionManager)
                    .AddSingleton<IExecutionContextManager, ExecutionContextManager>()
                    .AddSingleton<IRequestContextManager, RequestContextManager>()
                    .AddSingleton<IMongoDBClientFactory, MongoDBClientFactory>());

            var orleansClient = orleansBuilder.Build();
            services.AddSingleton(orleansClient);

            orleansClient.Connect(async (_) =>
            {
                await Task.Delay(1000);
                return true;
            }).Wait();
        }
    }
}
