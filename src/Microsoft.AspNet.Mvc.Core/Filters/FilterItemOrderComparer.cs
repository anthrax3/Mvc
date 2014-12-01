// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;

namespace Microsoft.AspNet.Mvc
{
    public class FilterItemOrderComparer : IComparer<FilterItem>
    {
        private static readonly FilterItemOrderComparer Instance = new FilterItemOrderComparer();

        public static FilterItemOrderComparer Comparer
        {
            get { return Instance; }
        }

        public int Compare([NotNull] FilterItem x, [NotNull] FilterItem y)
        {
            return FilterDescriptorOrderComparer.Comparer.Compare(x.Descriptor, y.Descriptor);
        }
    }
}
