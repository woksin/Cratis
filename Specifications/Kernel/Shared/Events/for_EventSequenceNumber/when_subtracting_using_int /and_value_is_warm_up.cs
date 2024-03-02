// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Events.for_EventSequenceNumber.when_subtracting_using_int;

public class and_value_is_warm_up : Specification
{
    EventSequenceNumber result;

    void Because() => result = EventSequenceNumber.WarmUp - 42;

    [Fact] void should_remain_warm_up() => result.ShouldEqual(EventSequenceNumber.WarmUp);
}
