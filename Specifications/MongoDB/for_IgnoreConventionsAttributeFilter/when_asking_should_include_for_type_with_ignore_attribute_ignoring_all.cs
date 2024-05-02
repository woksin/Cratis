// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using MongoDB.Bson.Serialization.Conventions;

namespace Cratis.MongoDB.for_IgnoreConventionsAttributeFilter;

public class when_asking_should_include_for_type_with_ignore_attribute_ignoring_all : Specification
{
    [IgnoreConventions]
    record TheType();

    IgnoreConventionsAttributeFilter filter;

    bool result;

    void Establish() => filter = new();

    void Because() => result = filter.ShouldInclude("SomePack", Mock.Of<IConventionPack>(), typeof(TheType));

    [Fact] void should_not_include_it() => result.ShouldBeFalse();
}
