// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events;
using Aksio.Cratis.EventSequences;

namespace Aksio.Cratis.Aggregates;

/// <summary>
/// Represents an implementation of <see cref="IAggregateRoot"/>.
/// </summary>
public class AggregateRoot : IAggregateRoot
{
    /// <summary>
    /// Cratis Internal: The event handlers for the aggregate root.
    /// </summary>
    internal AggregateRootEventHandlers EventHandlers = default!;

    /// <summary>
    /// Cratis Internal: The event sequence for the aggregate root.
    /// </summary>
    internal IEventSequence EventSequence = default!;

    /// <summary>
    /// Cratis Internal: The <see cref="EventSourceId"/> for the aggregate root.
    /// </summary>
    internal EventSourceId EventSourceId = EventSourceId.Unspecified;

    readonly List<object> _uncommittedEvents = new();

    /// <inheritdoc/>
    public virtual EventSequenceId EventSequenceId => EventSequenceId.Log;

    /// <summary>
    /// Cratis Internal: Whether or not the aggregate root is stateful.
    /// </summary>
    internal virtual bool IsStateful => false;

    /// <summary>
    /// Cratis Internal: The type of state for the aggregate root.
    /// </summary>
    internal virtual Type StateType => typeof(object);

    /// <inheritdoc/>
    public async Task Apply<T>(T @event)
        where T : class
    {
        typeof(T).ValidateEventType();
        _uncommittedEvents.Add(@event);
        await EventHandlers.Handle(this, new[] { new EventAndContext(@event, EventContext.Empty) });
    }

    /// <inheritdoc/>
    public Task Commit() => EventSequence.AppendMany(EventSourceId, _uncommittedEvents);

    /// <summary>
    /// Cratis Internal: Set the state for the aggregate root.
    /// </summary>
    /// <param name="state">State to set.</param>
    internal virtual void SetState(object state)
    {
    }
}

/// <summary>
/// Represents a stateful implementation of <see cref="IAggregateRoot"/>.
/// </summary>
/// <typeparam name="TState">Type of state for the aggregate root.</typeparam>
public class AggregateRoot<TState> : AggregateRoot
    where TState : class
{
    /// <summary>
    /// State of the aggregate root - accessible only to Cratis internally.
    /// </summary>
    internal TState _state = default!;

    /// <summary>
    /// Gets the current state of the aggregate root.
    /// </summary>
    protected TState State => _state;

    /// <inheritdoc/>
    internal override void SetState(object state) => _state = (state as TState)!;
}
