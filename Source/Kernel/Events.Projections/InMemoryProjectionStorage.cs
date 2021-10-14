// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;
using Cratis.Dynamic;

namespace Cratis.Events.Projections
{
    /// <summary>
    /// Represents an implementation of <see cref="IProjectionStorage"/> for working with projections in memory.
    /// </summary>
    public class InMemoryProjectionStorage : IProjectionStorage
    {
        readonly Dictionary<string, Dictionary<object, ExpandoObject>> _collections = new();

        /// <inheritdoc/>
        public Task<ExpandoObject> FindOrDefault(Model model, object key)
        {
            var collection = GetCollectionFor(model);

            ExpandoObject modelInstance;
            if (collection.ContainsKey(key))
            {
                modelInstance = collection[key];
            }
            else
            {
                modelInstance = new ExpandoObject();
            }

            return Task.FromResult(modelInstance);
        }

        /// <inheritdoc/>
        public Task ApplyChanges(Model model, object key, Changeset changeset)
        {
            var state = changeset.InitialState.Clone();

            foreach (var change in changeset.Changes)
            {
                state = state.OverwriteWith(change.State);
            }

            var collection = GetCollectionFor(model);
            collection[key] = state;

            return Task.CompletedTask;
        }

        Dictionary<object, ExpandoObject> GetCollectionFor(Model model)
        {
            Dictionary<object, ExpandoObject> collection;
            if (_collections.ContainsKey(model.Name))
            {
                collection = _collections[model.Name];
            }
            else
            {
                _collections[model.Name] = collection = new();
            }
            return collection;
        }
    }
}