// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;

namespace Cratis.Events.Projections.for_PropertyMappers
{
    public class when_mapping_to_a_deep_nested_property_from_an_event_value_provider : Specification
    {
        PropertyMapper property_mapper;
        Event @event;
        ExpandoObject result;
        Event provided_event;

        void Establish()
        {
            dynamic content = new ExpandoObject();
            result = new();
            @event = new Event(0, "02405794-91e7-4e4f-8ad1-f043070ca297", DateTimeOffset.UtcNow, "2f005aaf-2f4e-4a47-92ea-63687ef74bd4", content);
            property_mapper = PropertyMappers.FromEventValueProvider("deep.nested.property", _ =>
            {
                provided_event = _;
                return 42;
            });
        }

        void Because() => property_mapper(@event, result);

        [Fact] void should_set_value_in_expected_property() => ((object)((dynamic)result).deep.nested.property).ShouldEqual(42);
        [Fact] void should_pass_the_event_to_the_value_provider() => provided_event.ShouldEqual(@event);
    }
}