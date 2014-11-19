// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNet.Mvc;
using Microsoft.AspNet.Mvc.ModelBinding;

namespace ModelBindingWebSite.Controllers
{
    public class CustomModelBinderController : Controller
    {
        public Company GetCompany([ModelBinder(Name = "customPrefix")]Company company)
        {
            return company;
        }

        public string GetBinderType_UseModelBinderOnType([ModelBinder(Name = "customPrefix")] Model model)
        {
            return model.BinderType.FullName;
        }

        public string GetBinderType_UseModelBinderProviderOnType(
            [ModelBinder(Name = "customPrefix")] Model_WithProviderType model)
        {
            return model.BinderType.FullName;
        }

        public string GetBinderType_UseModelBinder(
            [ModelBinder(BinderType = typeof(CustomTestModelBinder))] Model model)
        {
            return model.BinderType.FullName;
        }

        public string GetBinderType_UseModelBinderProvider(
            [ModelBinder(BinderType = typeof(CustomTestModelBinderProvider))] Model model)
        {
            return model.BinderType.FullName;
        }



        [ModelBinder(BinderType = typeof(CustomTestModelBinder))]
        public class Model : IBinderTypeProvider
        {
            public Type BinderType { get; set; }
        }

        [ModelBinder(BinderType = typeof(CustomTestModelBinderProvider))]
        public class Model_WithProviderType : IBinderTypeProvider
        {
            public Type BinderType { get; set; }
        }
    }
}