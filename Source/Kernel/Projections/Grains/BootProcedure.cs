// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Boot;
using Aksio.Cratis.Execution;
using Orleans;

namespace Aksio.Cratis.Events.Projections.Grains
{
    /// <summary>
    /// Represents a <see cref="IPerformBootProcedure"/> for the event store.
    /// </summary>
    public class BootProcedure : IPerformBootProcedure
    {
        readonly IGrainFactory _grainFactory;
        readonly IExecutionContextManager _executionContextManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="BootProcedure"/> class.
        /// </summary>
        /// <param name="grainFactory"><see cref="IGrainFactory"/> for getting grains.</param>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with the execution context.</param>
        public BootProcedure(IGrainFactory grainFactory, IExecutionContextManager executionContextManager)
        {
            _grainFactory = grainFactory;
            _executionContextManager = executionContextManager;
        }

        /// <inheritdoc/>
        public void Perform()
        {
            // TODO: Start for all tenants
            _executionContextManager.Establish(
                Guid.Parse("3352d47d-c154-4457-b3fb-8a2efb725113"),
                Guid.NewGuid().ToString());

            var projections = _grainFactory.GetGrain<IProjections>(Guid.Empty);
            projections.Start();
        }
    }
}
