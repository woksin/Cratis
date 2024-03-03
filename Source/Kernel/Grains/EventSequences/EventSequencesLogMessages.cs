// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.EventSequences;
using Microsoft.Extensions.Logging;

namespace Cratis.Kernel.Grains.EventSequences;

internal static partial class EventSequencesLogMessages
{
    [LoggerMessage(0, LogLevel.Error, "Failed when rehydrating event sequence {EventSequenceId} for microservice {MicroserviceId} and {TenantId}")]
    internal static partial void FailedRehydratingEventSequence(this ILogger<EventSequences> logger, EventSequenceId eventSequenceId, MicroserviceId microserviceId, TenantId tenantId, Exception exception);
}