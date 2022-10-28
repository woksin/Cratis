// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Linq.Expressions;
using Aksio.Cratis.Events.Projections.Definitions;

namespace Aksio.Cratis.Events.Projections;

/// <summary>
/// Defines the builder for building out a <see cref="IProjectionFor{TModel}"/>.
/// </summary>
/// <typeparam name="TModel">Type of model.</typeparam>
public interface IProjectionBuilderFor<TModel>
{
    /// <summary>
    /// Names the projection. Default behavior is to use the type of the models full name.
    /// </summary>
    /// <param name="name">The name of the projection to use.</param>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> WithName(string name);

    /// <summary>
    /// Names the model - typically used by storage as name of storage unit (collection, table etc.)
    /// </summary>
    /// <param name="modelName">Name of the model.</param>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> ModelName(string modelName);

    /// <summary>
    /// Set the projection to not be rewindable - its a moving forward only projection.
    /// </summary>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> NotRewindable();

    /// <summary>
    /// Sets the initial state to use for new instances of the model.
    /// </summary>
    /// <param name="initialModelStateProviderCallback">Callback for providing an instance of the model representing the initial state.</param>
    /// <returns>Builder continuation.</returns>
    /// <remarks>
    /// If one does not provide initial state, the projection engine will leave properties
    /// out that hasn't been met by an event projection expression. This will effectively render
    /// the properties null and might not be desirable when reading instances of the models.
    /// </remarks>
    IProjectionBuilderFor<TModel> WithInitialModelState(Func<TModel> initialModelStateProviderCallback);

    /// <summary>
    /// Start building a from expressions for a specific event type.
    /// </summary>
    /// <param name="builderCallback">Callback for building.</param>
    /// <typeparam name="TEvent">Type of event.</typeparam>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> From<TEvent>(Action<IFromBuilder<TModel, TEvent>> builderCallback);

    /// <summary>
    /// Start building a join expressions for a specific event type.
    /// </summary>
    /// <param name="builderCallback">Callback for building.</param>
    /// <typeparam name="TEvent">Type of event.</typeparam>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> Join<TEvent>(Action<IJoinBuilder<TModel, TEvent>> builderCallback);

    /// <summary>
    /// Start building property expressions that applies for all events being projected from.
    /// </summary>
    /// <param name="builderCallback">Callback for building.</param>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> All(Action<IAllBuilder<TModel>> builderCallback);

    /// <summary>
    /// Define an event type that causes a delete in the projected result.
    /// </summary>
    /// <typeparam name="TEvent">Type of event.</typeparam>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> RemovedWith<TEvent>();

    /// <summary>
    /// Start building the children projection for a specific child model.
    /// </summary>
    /// <param name="targetProperty">Expression for expressing the target property.</param>
    /// <param name="builderCallback">Builder callback.</param>
    /// <typeparam name="TChildModel">Type of child model.</typeparam>
    /// <returns>Builder continuation.</returns>
    IProjectionBuilderFor<TModel> Children<TChildModel>(Expression<Func<TModel, IEnumerable<TChildModel>>> targetProperty, Action<IChildrenBuilder<TModel, TChildModel>> builderCallback);

    /// <summary>
    /// Build a <see cref="ProjectionDefinition"/>.
    /// </summary>
    /// <returns>A new <see cref="ProjectionDefinition"/>.</returns>
    ProjectionDefinition Build();
}
