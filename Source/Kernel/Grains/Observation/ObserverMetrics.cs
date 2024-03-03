// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Kernel.Keys;
using Cratis.Metrics;
using Cratis.Observation;

namespace Cratis.Kernel.Grains.Observation;

#pragma warning disable SA1600 // Elements should be documented
#pragma warning disable MA0048 // File name must match type name
#pragma warning disable SA1402 // File may only contain a single type

internal static partial class ObserverMetrics
{
    [Counter<int>("cratis-observer-partition-failed", "Number of times we have failed partitions")]
    internal static partial void PartitionFailed(this IMeterScope<Observer> meter, Key partition);
}

internal static class ObserverMetricsScopes
{
    internal static IMeterScope<Observer> BeginObserverScope(this IMeter<Observer> meter, ObserverId observerId, ObserverKey observerKey) =>
        meter.BeginScope(new Dictionary<string, object>
        {
            ["ObserverId"] = observerId,
            ["MicroserviceId"] = observerKey.MicroserviceId,
            ["TenantId"] = observerKey.TenantId,
            ["EventSequenceId"] = observerKey.EventSequenceId
        });
}