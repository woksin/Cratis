// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Newtonsoft.Json.Schema;

namespace Cratis.Events.Projections.for_Projections
{
    public class when_next_event_is_not_of_interest : Specification
    {
        Projection projection;
        bool observed;

        void Establish()
        {
            projection = new Projection(
                "0b7325dd-7a25-4681-9ab7-c387a6073547",
                new Model(string.Empty, new JSchema()),
                new[] {
                    new EventTypeWithKeyResolver("aac3d310-ff2f-4809-a326-afe14dd9a3d6", KeyResolvers.EventSourceId)
                });
            projection.Event.Subscribe(_ => observed = true);
        }

        void Because() => projection.OnNext(
            new Event(
                0,
                "5eb35b73-527b-47df-a6a0-20609930836f",
                DateTimeOffset.UtcNow,
                "30c1ebf5-cc30-4216-afed-e3e0aefa1316",
                new()), new());

        [Fact] void should_not_be_observed() => observed.ShouldBeFalse();
    }
}