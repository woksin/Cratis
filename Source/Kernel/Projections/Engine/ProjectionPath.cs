// Copyright (c) Cratis. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Aksio.Cratis.Events.Projections
{
    /// <summary>
    /// Represents the path for a <see cref="Projection"/>.
    /// </summary>
    /// <param name="Path">The path string.</param>
    public record ProjectionPath(string Path) : ConceptAs<string>(Path)
    {
        /// <summary>
        /// Implicitly convert from <see cref="string"/> to <see cref="ProjectionPath"/>.
        /// </summary>
        /// <param name="path">String path.</param>
        public static implicit operator ProjectionPath(string path) => new(path);
    }
}
