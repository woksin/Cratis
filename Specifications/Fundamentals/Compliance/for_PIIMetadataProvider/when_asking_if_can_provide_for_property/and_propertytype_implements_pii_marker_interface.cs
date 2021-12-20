// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Reflection;

namespace Cratis.Compliance.for_PIIMetadataProvider.when_asking_if_can_provide_for_property
{
    public class and_propertytype_implements_pii_marker_interface : given.a_provider
    {
        class MyType : IHoldPII { }

        class MyClass
        {
            public MyType Something { get; set; }

            public static PropertyInfo SomethingProperty = typeof(MyClass).GetProperty(nameof(Something), BindingFlags.Public | BindingFlags.Instance);
        }

        bool result;
        void Because() => result = provider.CanProvide(MyClass.SomethingProperty);

        [Fact] void should_be_able_to_provide() => result.ShouldBeTrue();
    }
}