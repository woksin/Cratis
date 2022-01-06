// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Cratis.Compliance.Concepts.PersonalInformation;

namespace Cratis.Compliance.Read.GDPR
{
    public record PersonalInformation(PersonalInformationType Type, PersonalInformationValue Value);
}
