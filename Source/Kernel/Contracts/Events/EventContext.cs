// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Kernel.Contracts.Auditing;
using Aksio.Cratis.Kernel.Contracts.Identities;
using Aksio.Cratis.Kernel.Contracts.Primitives;
using ProtoBuf;

namespace Aksio.Cratis.Kernel.Contracts.Events;

/// <summary>
/// Represents the context in which an event exists - typically what it was appended with.
/// </summary>
[ProtoContract]
public class EventContext
{
    /// <summary>
    /// Gets or sets the event source id.
    /// </summary>
    [ProtoMember(1)]
    public string EventSourceId { get; set; }

    /// <summary>
    /// Gets or sets the sequence number of the event as persisted in the event sequence.
    /// </summary>
    [ProtoMember(2)]
    public ulong SequenceNumber { get; set; }

    /// <summary>
    /// Gets or sets when it occurred.
    /// </summary>
    [ProtoMember(3)]
    public SerializableDateTimeOffset Occurred { get; set; }

    /// <summary>
    /// Gets or sets when event is considered valid from.
    /// </summary>
    [ProtoMember(4)]
    public SerializableDateTimeOffset ValidFrom { get; set; }

    /// <summary>
    /// Gets or sets the tenant id the event was appended to.
    /// </summary>
    [ProtoMember(5)]
    public Guid TenantId { get; set; }

    /// <summary>
    /// Gets or sets the correlation id for the event.
    /// </summary>
    [ProtoMember(6)]
    public string CorrelationId { get; set; }

    /// <summary>
    /// Gets or sets a collection of causation for what caused the event.
    /// </summary>
    [ProtoMember(7)]
    public IEnumerable<Causation> Causation { get; set; }

    /// <summary>
    /// Gets or sets a collection of Identities that caused the event.
    /// </summary>
    [ProtoMember(8)]
    public Identity CausedBy { get; set; }
}
