// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Core;
using Microsoft.AspNet.Mvc.ModelBinding;
using Microsoft.Framework.DependencyInjection;

namespace Microsoft.AspNet.Mvc
{
    /// <summary>
    /// Represents an <see cref="IModelBinder"/> which understands <see cref="ICustomModelBinderMetadata"/> and uses
    /// the supplied <see cref="IModelBinder"/> bind the model.
    /// </summary>
    public class CustomModelBinder : MetadataAwareBinder<ICustomModelBinderMetadata>
    {
        private readonly ITypeActivator _typeActivator;
        public CustomModelBinder([NotNull] ITypeActivator typeActivator)
        {
            _typeActivator = typeActivator;
        }

        protected override async Task<bool> BindAsync(ModelBindingContext bindingContext,
                                                      ICustomModelBinderMetadata metadata)
        {
            if (bindingContext.ModelMetadata.BinderType == null)
            {
                // Return false so that we are able to continue with the default set of model binders,
                // if there is no specific model binder provided.
                return false;
            }

            var requestServices = bindingContext.OperationBindingContext.HttpContext.RequestServices;
            var instance = _typeActivator.CreateInstance(requestServices, bindingContext.ModelMetadata.BinderType);
            var modelBinder = instance as IModelBinder;
            if (modelBinder == null)
            {
                var modelBinderProvider = instance as IModelBinderProvider;
                if (modelBinderProvider != null)
                {
                    modelBinder = new CompositeModelBinder(modelBinderProvider);
                }
                else
                {
                    throw new InvalidOperationException(
                        Resources.FormatModelBinderAttribute_TypeMustDeriveFromTypeOrType(
                            bindingContext.ModelMetadata.BinderType.FullName,
                            typeof(IModelBinder).FullName,
                            typeof(IModelBinderProvider).FullName));
                }
            }

            await modelBinder.BindModelAsync(bindingContext);

            // Irrespective of the result of the model binding,
            // since this class is supposed to handle ICustomModelBinderMetadata return true.
            return true;
        }
    }
}
