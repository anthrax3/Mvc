// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace ModelBindingWebSite
{
    public class CustomTestModelBinderProvider : IModelBinderProvider
    {
        public IReadOnlyList<IModelBinder> ModelBinders
        {
            get
            {
                return new[] { new CustomTestModelBinder() };
            }
        }
    }
}