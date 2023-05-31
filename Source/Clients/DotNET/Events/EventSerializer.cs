// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.Extensions.DependencyInjection;

namespace Aksio.Cratis.Events;

/// <summary>
/// Represents an implementation of <see cref="IEventSerializer"/>.
/// </summary>
[Singleton]
public class EventSerializer : IEventSerializer
{
    readonly IClientArtifactsProvider _clientArtifacts;
    readonly IServiceProvider _serviceProvider;
    readonly JsonSerializerOptions _serializerOptions;

    /// <summary>
    /// Initializes a new instance of the <see cref="EventSerializer"/> class.
    /// </summary>
    /// <param name="clientArtifacts">Optional <see cref="IClientArtifactsProvider"/> for the client artifacts.</param>
    /// <param name="serviceProvider"><see cref="IServiceProvider"/> for resolving instances.</param>
    /// <param name="eventTypes"><see cref="IEventTypes"/> for resolving event types.</param>
    /// <param name="serializerOptions">The common <see creF="JsonSerializerOptions"/>.</param>
    public EventSerializer(
        IClientArtifactsProvider clientArtifacts,
        IServiceProvider serviceProvider,
        IEventTypes eventTypes,
        JsonSerializerOptions serializerOptions)
    {
        _clientArtifacts = clientArtifacts;
        _serviceProvider = serviceProvider;
        _serializerOptions = new JsonSerializerOptions(serializerOptions)
        {
            Converters =
            {
                new EventRedactedConverter(eventTypes)
            }
        };
    }

    /// <inheritdoc/>
    public Task<object> Deserialize(Type type, JsonObject json) => Task.FromResult(json.Deserialize(type, _serializerOptions)!);

    /// <inheritdoc/>
    public async Task<JsonObject> Serialize(object @event)
    {
        var eventAsJson = (JsonSerializer.SerializeToNode(@event, _serializerOptions) as JsonObject)!;

        foreach (var providerType in _clientArtifacts.AdditionalEventInformationProviders)
        {
            var provider = (_serviceProvider.GetRequiredService(providerType) as ICanProvideAdditionalEventInformation)!;
            await provider.ProvideFor(eventAsJson);
        }

        return eventAsJson;
    }
}
