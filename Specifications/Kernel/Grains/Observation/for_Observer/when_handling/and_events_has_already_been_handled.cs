// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.for_Observer.when_handling;

[Collection(OrleansClusterCollection.Name)]
public class and_events_has_already_been_handled : given.an_observer_with_subscription_for_specific_event_type
{
    public and_events_has_already_been_handled(OrleansClusterFixture clusterFixture)
        : base(clusterFixture)
    {
    }

    void Establish()
    {
        state = state with
        {
            NextEventSequenceNumber = 53UL,
            LastHandledEventSequenceNumber = 54UL
        };
    }

    async Task Because() => await observer.Handle("Something", new[] { AppendedEvent.EmptyWithEventTypeAndEventSequenceNumber(event_type, 43UL) });

    [Fact] void should_not_forward_to_subscriber() => subscriber.Verify(_ => _.OnNext(IsAny<IEnumerable<AppendedEvent>>(), IsAny<ObserverSubscriberContext>()), Never);
    [Fact] void should_not_set_next_sequence_number() => state.NextEventSequenceNumber.ShouldEqual((EventSequenceNumber)53UL);
    [Fact] void should_not_set_last_handled_event_sequence_number() => state.LastHandledEventSequenceNumber.ShouldEqual((EventSequenceNumber)54UL);
    [Fact] void should_not_write_state() => written_states.Count.ShouldEqual(0);
}
