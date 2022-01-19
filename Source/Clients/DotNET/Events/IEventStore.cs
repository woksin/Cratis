// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Events
{
    /// <summary>
    /// Defines the store that holds events.
    /// </summary>
    public interface IEventStore
    {
        /// <summary>
        /// Gets the default event log.
        /// </summary>
        IEventLog EventLog { get; }
    }
}
