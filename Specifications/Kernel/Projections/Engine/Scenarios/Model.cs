// Copyright (c) Aksio Insurtech. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Aksio.Cratis.Events.Projections.Scenarios.Concepts;

namespace Aksio.Cratis.Events.Projections.Scenarios;

public record Model(
    string StringValue,
    bool BoolValue,
    int IntValue,
    float FloatValue,
    double DoubleValue,
    EnumWithValues EnumValue,
    Guid GuidValue,
    DateTime DateTimeValue,
    DateOnly DateOnlyValue,
    TimeOnly TimeOnlyValue,
    DateTimeOffset DateTimeOffsetValue,
    StringConcept StringConceptValue,
    BoolConcept BoolConceptValue,
    IntConcept IntConceptValue,
    FloatConcept FloatConceptValue,
    DoubleConcept DoubleConceptValue,
    EnumWithValuesConcept EnumConceptValue,
    GuidConcept GuidConceptValue,
    DateTimeConcept DateTimeConceptValue,
    DateOnlyConcept DateOnlyConceptValue,
    TimeOnlyConcept TimeOnlyConceptValue,
    DateTimeOffsetConcept DateTimeOffsetConceptValue,
    DateTimeOffset LastUpdated)
{
    public static Model CreateWithKnownValues() => new(
            KnownValues.StringValue,
            KnownValues.BoolValue,
            KnownValues.IntValue,
            KnownValues.FloatValue,
            KnownValues.DoubleValue,
            KnownValues.EnumValue,
            KnownValues.GuidValue,
            KnownValues.DateTimeValue,
            KnownValues.DateOnlyValue,
            KnownValues.TimeOnlyValue,
            KnownValues.DateTimeOffsetValue,
            KnownValues.StringConceptValue,
            KnownValues.BoolConceptValue,
            KnownValues.IntConceptValue,
            KnownValues.FloatConceptValue,
            KnownValues.DoubleConceptValue,
            KnownValues.EnumConceptValue,
            KnownValues.GuidConceptValue,
            KnownValues.DateTimeConceptValue,
            KnownValues.DateOnlyConceptValue,
            KnownValues.TimeOnlyConceptValue,
            KnownValues.DateTimeOffsetConceptValue,
            DateTimeOffset.UtcNow);
}
