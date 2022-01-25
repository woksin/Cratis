// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Concurrent;
using Aksio.Cratis.Events.Projections.Definitions;
using Aksio.Cratis.Events.Projections.Pipelines;
using Aksio.Cratis.Execution;
using Aksio.Cratis.Reactive;
using Microsoft.Extensions.Logging;

#pragma warning disable CA1721 // Pipelines property is confusing since there is a GetPipelines - they do differ - see GH issue #103.

namespace Aksio.Cratis.Events.Projections
{
    /// <summary>
    /// Represents an implementation of <see cref="IProjections"/>.
    /// </summary>
    [Singleton]
    public class Projections : IProjections
    {
        readonly IProjectionDefinitions _projectionDefinitions;
        readonly IProjectionPipelineDefinitions _projectionPipelineDefinitions;
        readonly IProjectionFactory _projectionFactory;
        readonly IProjectionPipelineFactory _pipelineFactory;
        readonly ILogger<Projections> _logger;
        readonly ConcurrentDictionary<ProjectionId, CancellationTokenSource> _cancellationTokenSources = new();
        readonly ObservableCollection<IProjectionPipeline> _allPipelines = new();

        /// <inheritdoc/>
        public IObservable<IEnumerable<IProjectionPipeline>> Pipelines => _allPipelines;

        /// <summary>
        /// Initializes a new instance of the <see cref="Projections"/> class.
        /// </summary>
        /// <param name="projectionDefinitions">The <see cref="IProjectionDefinitions"/> to use.</param>
        /// <param name="projectionPipelineDefinitions">The <see cref="IProjectionPipelineDefinitions"/> to use.</param>
        /// <param name="projectionFactory">The <see cref="IProjectionFactory"/> for creating projection instances.</param>
        /// <param name="pipelineFactory">The <see cref="IProjectionPipelineFactory"/> for creating projection pipeline instances.</param>
        /// <param name="logger"><see cref="ILogger"/> for logging.</param>
        public Projections(
            IProjectionDefinitions projectionDefinitions,
            IProjectionPipelineDefinitions projectionPipelineDefinitions,
            IProjectionFactory projectionFactory,
            IProjectionPipelineFactory pipelineFactory,
            ILogger<Projections> logger)
        {
            _projectionDefinitions = projectionDefinitions;
            _projectionPipelineDefinitions = projectionPipelineDefinitions;
            _projectionFactory = projectionFactory;
            _pipelineFactory = pipelineFactory;
            _logger = logger;
        }

        /// <inheritdoc/>
        public async Task Register(ProjectionDefinition projectionDefinition, ProjectionPipelineDefinition pipelineDefinition)
        {
            var projection = await _projectionFactory.CreateFrom(projectionDefinition);
            var pipeline = _pipelineFactory.CreateFrom(projection, pipelineDefinition);
            var isNew = !await _projectionDefinitions.HasFor(projectionDefinition.Identifier);
            var hasChanged = await _projectionDefinitions.HasChanged(projectionDefinition);
            var existingPipeline = _allPipelines.FirstOrDefault(_ => _.Projection.Identifier == projection.Identifier);
            if (!isNew && hasChanged && projection.IsRewindable)
            {
                await pipeline.Rewind();
            }

            if (existingPipeline != default && !projection.IsPassive)
            {
                await existingPipeline.Suspend("Replacing projection due to definition changes");
            }

            await _projectionDefinitions.Register(projectionDefinition);
            await _projectionPipelineDefinitions.Register(pipelineDefinition);

            if (!projection.IsPassive)
            {
                RunProjection(pipeline);
            }

            if (_allPipelines.Any(_ => _.Projection.Identifier == pipeline.Projection.Identifier))
            {
                var existing = _allPipelines.First(_ => _.Projection.Identifier == pipeline.Projection.Identifier);
                _allPipelines.Remove(existing);
            }

            _allPipelines.Add(pipeline);
        }

        /// <inheritdoc/>
        public async Task Start()
        {
            await RegisterUnregisteredProjections();
        }

        /// <inheritdoc/>
        public IEnumerable<IProjectionPipeline> GetPipelines() => _allPipelines;

        /// <inheritdoc/>
        public IProjectionPipeline GetById(ProjectionId id) => _allPipelines.Single(_ => _.Projection.Identifier == id);

        void RunProjection(IProjectionPipeline pipeline)
        {
            if (_cancellationTokenSources.ContainsKey(pipeline.Projection.Identifier))
            {
                _logger.StopRunning(pipeline.Projection.Identifier, pipeline.Projection.Name);
                _cancellationTokenSources[pipeline.Projection.Identifier].Cancel();
            }
            var cts = new CancellationTokenSource();
            _cancellationTokenSources[pipeline.Projection.Identifier] = cts;
            _logger.Running(pipeline.Projection.Identifier, pipeline.Projection.Name);
            Task.Run(async () => await pipeline.Start(), cts.Token);
        }

        async Task RegisterUnregisteredProjections()
        {
            var projectionPipelineDefinitions = await _projectionPipelineDefinitions.GetAll();
            foreach (var pipeline in projectionPipelineDefinitions.Where(pipeline => !_allPipelines.Any(_ => _.Projection.Identifier == pipeline.ProjectionId)))
            {
                if (await _projectionDefinitions.HasFor(pipeline.ProjectionId))
                {
                    var projection = await _projectionDefinitions.GetFor(pipeline.ProjectionId);
                    await Register(projection, pipeline);
                }
            }
        }
    }
}