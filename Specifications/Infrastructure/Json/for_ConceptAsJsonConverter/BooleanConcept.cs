// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Concepts;

namespace Cratis.Json.for_ConceptAsJsonConverter;

public record BooleanConcept(bool Value) : ConceptAs<bool>(Value);
