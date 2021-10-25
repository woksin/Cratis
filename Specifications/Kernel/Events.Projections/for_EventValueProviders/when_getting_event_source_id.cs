// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;

namespace Cratis.Events.Projections.for_PropertyMappers
{
    public class when_getting_event_source_id : Specification
    {
        static EventSourceId    eventSourceId = "2f005aaf-2f4e-4a47-92ea-63687ef74bd4";

        EventValueProvider value_provider;
        Event @event;
        EventSourceId result;

        void Establish()
        {
            dynamic content = new ExpandoObject();
            @event = new Event(0, "02405794-91e7-4e4f-8ad1-f043070ca297", DateTimeOffset.UtcNow, eventSourceId, content);
            value_provider = EventValueProviders.FromEventSourceId();
        }

        void Because() => result = value_provider(@event) as EventSourceId;

        [Fact] void should_return_the_event_source_id() => result.ShouldEqual(eventSourceId);
    }
}