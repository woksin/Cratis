// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Concepts;
using Aksio.Cratis.Events.Store.Observation;

namespace Aksio.Cratis.Events.Observation
{
    /// <summary>
    /// Concept that represents the name of an observer.
    /// </summary>
    /// <param name="Value">Actual value.</param>
    public record ObserverName(string Value) : ConceptAs<string>(Value)
    {
        /// <summary>
        /// Implicitly convert from a string to <see cref="ObserverId"/>.
        /// </summary>
        /// <param name="id">String  to convert from.</param>
        public static implicit operator ObserverName(string id) => new(id);
    }
}