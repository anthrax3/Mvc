// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Globalization;
using System.Threading.Tasks;
using Microsoft.AspNet.PipelineCore;
using Microsoft.AspNet.Testing;
using Microsoft.Framework.DependencyInjection;
using Moq;
using Xunit;

namespace Microsoft.AspNet.Mvc.ModelBinding.Test
{
    public class CustomModelBinderTest
    {
        [Fact]
        public async Task BindModel_ReturnsFalseIfNoBinderTypeIsSet()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(Person));

            var binder = new CustomModelBinder(Mock.Of<ITypeActivator>());

            // Act
            var binderResult = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.False(binderResult);
        }

        [Fact]
        public async Task BindModel_ReturnsTrueEvenIfSelectedBinderReturnsFalse()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(Model));
            var mockModelBinder = new Mock<IModelBinder>();
            mockModelBinder.Setup(o => o.BindModelAsync(It.IsAny<ModelBindingContext>()))
                           .Returns(Task.FromResult(false))
                           .Verifiable();

            var mockITypeActivator = new Mock<ITypeActivator>();
            mockITypeActivator.Setup(o => o.CreateInstance(It.IsAny<IServiceProvider>(), It.IsAny<Type>()))
                               .Returns(mockModelBinder.Object);

            bindingContext.ModelMetadata.BinderType = typeof(IModelBinder);
            var binder = new CustomModelBinder(mockITypeActivator.Object);

            // Act
            var binderResult = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.True(binderResult);
            mockModelBinder.Verify();
        }

        [Fact]
        public async Task BindModel_CallsBindAsync_OnProvidedModelBinder()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(Model));
            var mockModelBinder = new Mock<IModelBinder>();
            mockModelBinder.Setup(o => o.BindModelAsync(It.IsAny<ModelBindingContext>()))
                           .Returns(Task.FromResult(true))
                           .Verifiable();

            var mockITypeActivator = new Mock<ITypeActivator>();
            mockITypeActivator.Setup(o => o.CreateInstance(It.IsAny<IServiceProvider>(), It.IsAny<Type>()))
                               .Returns(mockModelBinder.Object);

            bindingContext.ModelMetadata.BinderType = typeof(IModelBinder);
            var binder = new CustomModelBinder(mockITypeActivator.Object);

            // Act
            var binderResult = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.True(binderResult);
            mockModelBinder.Verify();
        }

        [Fact]
        public async Task BindModel_CallsBindAsync_OnProvidedModelBinderProvider()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(Model));
            var mockModelBinder = new Mock<IModelBinder>();
            mockModelBinder.Setup(o => o.BindModelAsync(It.IsAny<ModelBindingContext>()))
                           .Returns(Task.FromResult(true))
                           .Verifiable();

            var mockModelBinderProvider = new Mock<IModelBinderProvider>();
            mockModelBinderProvider.SetupGet(o => o.ModelBinders)
                                   .Returns(new[] { mockModelBinder.Object });

            var mockITypeActivator = new Mock<ITypeActivator>();
            mockITypeActivator.Setup(o => o.CreateInstance(It.IsAny<IServiceProvider>(), It.IsAny<Type>()))
                               .Returns(mockModelBinderProvider.Object);

            bindingContext.ModelMetadata.BinderType = typeof(IModelBinderProvider);
            var binder = new CustomModelBinder(mockITypeActivator.Object);

            // Act
            var binderResult = await binder.BindModelAsync(bindingContext);

            // Assert
            Assert.True(binderResult);
            mockModelBinder.Verify();
        }

        [Fact]
        public async Task BindModel_ForNonModelBinderAndModelBinderProviderTypes_Throws()
        {
            // Arrange
            var bindingContext = GetBindingContext(typeof(Model));
            bindingContext.ModelMetadata.BinderType = typeof(string);
            var binder = new CustomModelBinder(Mock.Of<ITypeActivator>());

            // Act
            var ex = await Assert.ThrowsAsync<InvalidOperationException>(
                () => binder.BindModelAsync(bindingContext));

            // Assert
            Assert.Equal(string.Format("The type '{0}' must derive from either '{1}' or '{2}'.",
                                        typeof(string).FullName,
                                        typeof(IModelBinder).FullName,
                                        typeof(IModelBinderProvider).FullName),
                         ex.Message);
        }

        private static ModelBindingContext GetBindingContext(Type modelType)
        {
            var metadataProvider = new DataAnnotationsModelMetadataProvider();
            var operationBindingContext = new OperationBindingContext
            {
                MetadataProvider = metadataProvider,
                HttpContext = new DefaultHttpContext(),
            };

            var bindingContext = new ModelBindingContext
            {
                ModelMetadata = metadataProvider.GetMetadataForType(null, modelType),
                ModelName = "someName",
                ValueProvider = Mock.Of<IValueProvider>(),
                ModelState = new ModelStateDictionary(),
                OperationBindingContext = operationBindingContext,
            };

            bindingContext.ModelMetadata.BinderMetadata = Mock.Of<ICustomModelBinderMetadata>();
            return bindingContext;
        }

        private class Model
        {
            public string Value { get; set; }
        }

        private class FalseModelBinder : IModelBinder
        {
            public Task<bool> BindModelAsync(ModelBindingContext bindingContext)
            {
                return Task.FromResult(false);
            }
        }
    }
}
