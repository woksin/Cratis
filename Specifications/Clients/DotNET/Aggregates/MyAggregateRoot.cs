// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Aggregates;

public class MyAggregateRoot : AggregateRoot
{
    public FirstEventType FirstEventTypeInstance;
    public SecondEventType SecondEventTypeInstance;

    public void OnFirstEvent(FirstEventType @event)
    {
        FirstEventTypeInstance = @event;
    }

    public void OnSecondEvent(SecondEventType @event, EventContext context)
    {
        SecondEventTypeInstance = @event;
    }
}
