// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Execution;
using Orleans.Streams;

namespace Aksio.Cratis.Events.Store.MongoDB
{
    /// <summary>
    /// Represents an implementation of <see cref="IQueueAdapterCache"/> for MongoDB event log.
    /// </summary>
    public class EventLogQueueAdapterCache : IQueueAdapterCache
    {
        readonly IExecutionContextManager _executionContextManager;
        readonly IEventStoreDatabase _eventStoreDatabase;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventLogQueueAdapterCache"/> class.
        /// </summary>
        /// <param name="executionContextManager"><see cref="IExecutionContextManager"/> for working with execution context.</param>
        /// <param name="eventStoreDatabase">The <see cref="IEventStoreDatabase"/> to use.</param>
        public EventLogQueueAdapterCache(
            IExecutionContextManager executionContextManager,
            IEventStoreDatabase eventStoreDatabase)
        {
            _executionContextManager = executionContextManager;
            _eventStoreDatabase = eventStoreDatabase;
        }

        /// <inheritdoc/>
        public IQueueCache CreateQueueCache(QueueId queueId) => new EventLogQueueCache(_executionContextManager, _eventStoreDatabase);
    }
}
