// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Aggregates;

/// <summary>
/// Represents a system that is capable of creating instances of <see cref="IAggregateRootEventHandlersFactory"/>.
/// </summary>
public class AggregateRootEventHandlersFactory : IAggregateRootEventHandlersFactory
{
    /// <inheritdoc/>
    public IAggregateRootEventHandlers Create(Type type) => new AggregateRootEventHandlers(type);
}
