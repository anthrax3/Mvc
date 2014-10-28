﻿// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc.Razor
{
    /// <summary>
    /// Represents the results of locating a <see cref="IRazorPage"/>.
    /// </summary>
    public class RazorPageResult
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RazorPageResult"/> for a successful match.
        /// </summary>
        /// <param name="name">The name of the page being located.</param>
        /// <param name="page">The located <see cref="IRazorPage"/>.</param>
        public RazorPageResult([NotNull] string name, [NotNull] IRazorPage page)
        {
            Name = name;
            Page = page;
        }

        /// <summary>
        /// Initializes a new instance of <see cref="RazorPageResult"/> for an unsuccessful match.
        /// </summary>
        /// <param name="name">The name of the page being located.</param>
        /// <param name="page">The locations that were searched.</param>
        public RazorPageResult([NotNull] string name, [NotNull] IEnumerable<string> searchedLocations)
        {
            Name = name;
            SearchedLocations = searchedLocations;
        }

        /// <summary>
        /// Gets the name of the page being located.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the <see cref="IRazorPage"/> if found.
        /// </summary>
        public IRazorPage Page { get; }

        /// <summary>
        /// Gets the locations that were searched when <see cref="Page"/> could not be located.
        /// </summary>
        public IEnumerable<string> SearchedLocations { get; }
    }
}