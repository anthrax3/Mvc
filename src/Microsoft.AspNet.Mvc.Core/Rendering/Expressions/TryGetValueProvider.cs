// Copyright (c) Microsoft Open Technologies, Inc. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using System.Threading;

namespace Microsoft.AspNet.Mvc.Rendering.Expressions
{
    public static class TryGetValueProvider
    {
        private static readonly Dictionary<Type, TryGetValueDelegate> TryGetValueDelegateCache =
            new Dictionary<Type, TryGetValueDelegate>();
        private static readonly ReaderWriterLockSlim TryGetValueDelegateCacheLock = new ReaderWriterLockSlim();

        // Information about private static method declared below.
        private static readonly MethodInfo StrongTryGetValueImplInfo =
            typeof(TryGetValueProvider).GetTypeInfo().GetDeclaredMethod("StrongTryGetValueImpl");

        public static TryGetValueDelegate CreateInstance([NotNull] Type targetType)
        {
            TryGetValueDelegate result;

            // Cache delegates since properties of model types are re-evaluated numerous times.
            TryGetValueDelegateCacheLock.EnterReadLock();
            try
            {
                if (TryGetValueDelegateCache.TryGetValue(targetType, out result))
                {
                    return result;
                }
            }
            finally
            {
                TryGetValueDelegateCacheLock.ExitReadLock();
            }

            var dictionaryType = targetType.ExtractGenericInterface(typeof(IDictionary<,>));

            // Just wrap a call to the underlying IDictionary<TKey, TValue>.TryGetValue() where string can be cast to
            // TKey.
            if (dictionaryType != null)
            {
                var typeArguments = dictionaryType.GetGenericArguments();
                var keyType = typeArguments[0];
                var returnType = typeArguments[1];

                if (keyType.IsAssignableFrom(typeof(string)))
                {
                    var implementationMethod = StrongTryGetValueImplInfo.MakeGenericMethod(keyType, returnType);
                    result = (TryGetValueDelegate)implementationMethod.CreateDelegate(typeof(TryGetValueDelegate));
                }
            }

            // Wrap a call to the underlying IDictionary.Item().
            if (result == null && typeof(IDictionary).IsAssignableFrom(targetType))
            {
                result = TryGetValueFromNonGenericDictionary;
            }

            TryGetValueDelegateCacheLock.EnterWriteLock();
            try
            {
                TryGetValueDelegateCache[targetType] = result;
            }
            finally
            {
                TryGetValueDelegateCacheLock.ExitWriteLock();
            }

            return result;
        }

        private static bool StrongTryGetValueImpl<TKey, TValue>(object dictionary, string key, out object value)
        {
            var strongDict = (IDictionary<TKey, TValue>)dictionary;

            TValue strongValue;
            var success = strongDict.TryGetValue((TKey)(object)key, out strongValue);
            value = strongValue;
            return success;
        }

        private static bool TryGetValueFromNonGenericDictionary(object dictionary, string key, out object value)
        {
            var weakDict = (IDictionary)dictionary;

            var success = weakDict.Contains(key);
            value = success ? weakDict[key] : null;
            return success;
        }
    }
}