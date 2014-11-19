// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace ModelBindingWebSite
{
    public class CustomTestModelBinder : IModelBinder
    {
        public Task<bool> BindModelAsync(ModelBindingContext bindingContext)
        {
            if (typeof(IBinderTypeProvider).IsAssignableFrom(bindingContext.ModelType))
            {
                bindingContext.Model = Activator.CreateInstance(bindingContext.ModelType);
                ((IBinderTypeProvider)bindingContext.Model).BinderType = typeof(CustomTestModelBinder);
                return Task.FromResult(true);
            }

            return Task.FromResult(false);
        }
    }
}