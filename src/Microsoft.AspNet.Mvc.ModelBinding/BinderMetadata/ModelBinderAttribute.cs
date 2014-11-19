// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// An attribute that allows to specify an <see cref="IModelBinder"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class ModelBinderAttribute : Attribute, ICustomModelBinderMetadata, IModelNameProvider, IBinderTypeProvider
    {
        /// <inheritdoc />
        public Type BinderType { get; set; }

        /// <inheritdoc />
        public string Name { get; set; }
    }
}