// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Events.Projections.Expressions
{
    /// <summary>
    /// Represents a <see cref="IPropertyMapperExpressionResolver"/> for resolving value from a property on the content of an <see cref="Event"/>.
    /// </summary>
    public class PropertyOnEventContentExpressionProvider : IPropertyMapperExpressionResolver
    {
        /// <inheritdoc/>
        public bool CanResolve(string targetProperty, string expression) => !expression.StartsWith("$", StringComparison.InvariantCultureIgnoreCase);

        /// <inheritdoc/>
        public PropertyMapper Resolve(string targetProperty, string expression) => PropertyMappers.FromEventValueProvider(targetProperty, EventValueProviders.FromEventContent(expression));
    }
}