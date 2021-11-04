// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Cratis.Events.Projections.Expressions
{
    /// <summary>
    /// Represents an implementation of <see cref="IPropertyMapperExpressionResolvers"/>.
    /// </summary>
    public class PropertyMapperExpressionResolvers : IPropertyMapperExpressionResolvers
    {
        readonly IPropertyMapperExpressionResolver[] _resolvers = new IPropertyMapperExpressionResolver[]
        {
            new EventSourceIdExpressionResolver(),
            new AddExpressionResolver(),
            new SubtractExpressionResolver(),
            new PropertyOnEventContentExpressionProvider()
        };

        /// <inheritdoc/>
        public bool CanResolve(Property targetProperty, string expression) => _resolvers.Any(_ => _.CanResolve(targetProperty, expression));

        /// <inheritdoc/>
        public PropertyMapper Resolve(Property targetProperty, string expression)
        {
            var resolver = Array.Find(_resolvers, _ => _.CanResolve(targetProperty, expression));
            ThrowIfUnsupportedEventValueExpression(expression, resolver);
            return resolver!.Resolve(targetProperty, expression);
        }

        void ThrowIfUnsupportedEventValueExpression(string expression, IPropertyMapperExpressionResolver? resolver)
        {
            if (resolver == default)
            {
                throw new UnsupportedPropertyMapperExpression(expression);
            }
        }
    }
}
