// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.DependencyInversion;
using Cratis.Events;
using Cratis.Kernel.Storage.EventTypes;
using Cratis.Schemas;
using NJsonSchema;

namespace Cratis.Kernel.Grains.EventTypes;

/// <summary>
/// Represents an implementation of <see cref="IEventTypes"/>.
/// </summary>
public class EventTypes : Grain, IEventTypes
{
    readonly ProviderFor<IEventTypesStorage> _underlyingSchemaStore;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventTypes"/> class.
    /// </summary>
    /// <param name="underlyingSchemaStore"><see cref="IEventTypesStorage"/> underlying event types storage.</param>
    public EventTypes(ProviderFor<IEventTypesStorage> underlyingSchemaStore)
    {
        _underlyingSchemaStore = underlyingSchemaStore;
    }

    /// <inheritdoc/>
    public async Task Register(EventType type, string friendlyName, string schema)
    {
        var jsonSchema = await JsonSchema.FromJsonAsync(schema);
        jsonSchema.EnsureComplianceMetadata();
        await _underlyingSchemaStore().Register(type, friendlyName, jsonSchema);
    }
}