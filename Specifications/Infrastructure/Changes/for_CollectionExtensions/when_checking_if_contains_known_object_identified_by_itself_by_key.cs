// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Properties;

namespace Aksio.Cratis.Changes.for_CollectionExtensions;

public class when_checking_if_contains_known_object_identified_by_itself_by_key : Specification
{
    IEnumerable<string> items;
    bool result;

    void Establish() => items = new[]
    {
        "First",
        "Second"
    };

    void Because() => result = items.Contains(PropertyPath.Root, "Second");

    [Fact] void should_contain_the_object() => result.ShouldBeTrue();
}
