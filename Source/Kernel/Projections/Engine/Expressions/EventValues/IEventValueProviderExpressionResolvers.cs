// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Store;
using Aksio.Cratis.Properties;

namespace Aksio.Cratis.Events.Projections.Expressions.EventValues;

/// <summary>
/// Defines a system for resolving a event value provider expression. It represents known expression resolvers in the system.
/// </summary>
public interface IEventValueProviderExpressionResolvers
{
    /// <summary>
    /// Called to verify if the resolver can resolve the expression.
    /// </summary>
    /// <param name="expression">Expression to resolve.</param>
    /// <returns>True if it can resolve, false if not.</returns>
    bool CanResolve(string expression);

    /// <summary>
    /// Called to resolve the expression.
    /// </summary>
    /// <param name="expression">Expression to resolve.</param>
    /// <returns><see cref="PropertyMapper{Event, ExpandoObject}"/> it resolves to.</returns>
    ValueProvider<AppendedEvent> Resolve(string expression);
}
