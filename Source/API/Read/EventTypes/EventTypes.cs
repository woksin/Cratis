// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using Cratis.Events;
using Cratis.EventTypes;
using Cratis.Kernel.Storage;
using Microsoft.AspNetCore.Mvc;

namespace Cratis.Kernel.Read.EventTypes;

/// <summary>
/// Represents the API for working with event types.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="EventTypes"/> class.
/// </remarks>
/// <param name="storage"><see cref="IStorage"/> for accessing underlying storage.</param>
[Route("/api/events/store/{eventStore}/types")]
public class EventTypes(IStorage storage) : ControllerBase
{
    /// <summary>
    /// Gets all event types.
    /// </summary>
    /// <param name="eventStore">The <see cref="EventStoreName"/> to get event types for.</param>
    /// <returns>Collection of event types.</returns>
    [HttpGet]
    public async Task<IEnumerable<EventTypeInformation>> AllEventTypes([FromRoute] EventStoreName eventStore)
    {
        var eventTypes = await storage.GetEventStore(eventStore).EventTypes.GetLatestForAllEventTypes();
        return eventTypes.Select(_ =>
            new EventTypeInformation(
                _.Type.Id.ToString(),
                _.Schema.GetDisplayName(),
                _.Schema.GetGeneration()));
    }

    /// <summary>
    /// Gets generation schema for type.
    /// </summary>
    /// <param name="eventStore">The <see cref="EventStoreName"/> to get event type for.</param>
    /// <param name="eventTypeId">Type to get for.</param>
    /// <returns>Schemas.</returns>
    [HttpGet("schemas/{eventTypeId}")]
    public async Task<IEnumerable<JsonDocument>> GenerationSchemasForType(
        [FromQuery] EventStoreName eventStore,
        [FromRoute] EventTypeId eventTypeId)
    {
        var schemas = await storage.GetEventStore(eventStore).EventTypes.GetAllGenerationsForEventType(new(eventTypeId, 1));
        return schemas.Select(_ => JsonDocument.Parse(_.Schema.ToJson()));
    }

    /// <summary>
    /// Gets all event types with schemas.
    /// </summary>
    /// <param name="eventStore">The <see cref="EventStoreName"/> to get event types for.</param>
    /// <returns>Collection of event types with schemas.</returns>
    [HttpGet("schemas")]
    public async Task<IEnumerable<EventTypeWithSchemas>> AllEventTypesWithSchemas([FromRoute] EventStoreName eventStore)
    {
        var schemas = await storage.GetEventStore(eventStore).EventTypes.GetLatestForAllEventTypes();

        return schemas.Select(_ =>
            new EventTypeWithSchemas(
                new EventTypeInformation(
                    _.Type.Id.ToString(),
                    _.Schema.GetDisplayName(),
                    _.Schema.GetGeneration()),
                new[] { JsonDocument.Parse(_.Schema.ToJson()) }));
    }
}