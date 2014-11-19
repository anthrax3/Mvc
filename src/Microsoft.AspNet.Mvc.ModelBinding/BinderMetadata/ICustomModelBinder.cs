// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    /// <summary>
    /// Metadata interface that indicates model binding should be performed by a custom model binder.
    /// </summary>
    public interface ICustomModelBinderMetadata : IBinderMetadata
    {
    }
}
