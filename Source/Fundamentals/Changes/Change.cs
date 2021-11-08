// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Dynamic;

namespace Cratis.Changes
{
    /// <summary>
    /// Defines a change as part of a <see cref="Changeset{T}"/>.
    /// </summary>
    /// <param name="State">State after change applied.</param>
    public record Change(ExpandoObject State);
}