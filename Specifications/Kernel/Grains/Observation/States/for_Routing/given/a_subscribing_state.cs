// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.EventSequences;
using Aksio.Cratis.Kernel.Orleans.StateMachines;
using IEventSequence = Aksio.Cratis.Kernel.Grains.EventSequences.IEventSequence;

namespace Aksio.Cratis.Kernel.Grains.Observation.States.for_Routing.given;

public class a_routing_state : Specification
{
    protected Mock<IObserver> observer;
    protected Mock<IEventSequence> event_sequence;
    protected Mock<IStateMachine<ObserverState>> state_machine;
    protected Routing state;
    protected ObserverState stored_state;
    protected ObserverState resulting_stored_state;
    protected TailEventSequenceNumbers tail_event_sequence_numbers;
    protected ObserverSubscription subscription;

    void Establish()
    {
        observer = new();
        event_sequence = new();
        state = new Routing(observer.Object, event_sequence.Object);
        state_machine = new();
        state.SetStateMachine(state_machine.Object);
        stored_state = new ObserverState
        {
            RunningState = ObserverRunningState.Routing,
        };

        subscription = new ObserverSubscription(
            Guid.NewGuid(),
            new(MicroserviceId.Unspecified, TenantId.Development, EventSequenceId.Log),
            Enumerable.Empty<EventType>(),
            typeof(object),
            string.Empty);

        observer.Setup(_ => _.GetSubscription()).Returns(() => Task.FromResult(subscription));

        event_sequence.Setup(_ => _.GetTailSequenceNumber()).Returns(() => Task.FromResult(EventSequenceNumber.First));
        event_sequence.Setup(_ => _.GetTailSequenceNumberForEventTypes(It.IsAny<IEnumerable<EventType>>())).Returns(() => Task.FromResult(EventSequenceNumber.First));
    }
}
