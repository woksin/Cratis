// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Kernel.Grains.Observation.for_ObserverSupervisorState;

public class when_asking_if_there_are_failed_partitions_with_one_failed : given.a_failed_partition
{
    bool result;

    void Because() => result = state.HasFailedPartitions;

    [Fact] void should_have_failed_partitions() => result.ShouldBeTrue();
}