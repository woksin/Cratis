// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Events.Observation
{
    /// <summary>
    /// Defines the system for working with observers.
    /// </summary>
    public interface IObservers
    {
        /// <summary>
        /// Start observing for all observers.
        /// </summary>
        /// <returns>Awaitable task.</returns>
        Task StartObserving();
    }
}
