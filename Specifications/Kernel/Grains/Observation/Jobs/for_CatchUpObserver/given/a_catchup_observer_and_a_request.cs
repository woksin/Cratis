// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.Jobs.for_CatchUpObserver.given;

public class a_catchup_observer_and_a_request : a_catchup_observer
{
    public a_catchup_observer_and_a_request(OrleansClusterFixture clusterFixture)
        : base(clusterFixture)
    {
    }

    void Establish()
    {
        state.Request = new(
            Guid.NewGuid(),
            new ObserverKey(Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()),
            ObserverSubscription.Unsubscribed,
            42UL,
            new[] {
                new EventType(Guid.NewGuid(), EventGeneration.First),
                new EventType(Guid.NewGuid(), EventGeneration.First)
            });
    }
}
