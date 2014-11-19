// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace Microsoft.AspNet.Mvc.ModelBinding
{
    /// <summary>
    /// Represents an entity which can provide a <see cref="Type"/> which implements <see cref="IModelBinder"/> or
    /// <see cref="IModelBinderProvider"/> as metadata.
    /// </summary>
    public interface IBinderTypeProvider
    {
        /// <summary>
        /// Represents a <see cref="Type"/> which implements either <see cref="IModelBinder"/> or
        /// <see cref="IModelBinderProvider"/>.
        /// </summary>
        Type BinderType { get; set; }
    }
}
