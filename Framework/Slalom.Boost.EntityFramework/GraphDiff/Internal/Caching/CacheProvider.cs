﻿using System;
using System.Linq;
using System.Runtime.Caching;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Caching
{
    internal class CacheProvider : ICacheProvider
    {
        private static readonly MemoryCache Cache = MemoryCache.Default;
        private static readonly object CacheLock = new object();

        public void Insert(string register, string key, object value)
        {
            lock (CacheLock)
            {
                var fullKey = GenerateKey(register, key);
                var result = Cache.Get(fullKey);
                if (result == null)
                {
                    Cache.Add(fullKey, value, new CacheItemPolicy());
                }
            }
        }

        public void Clear(string register)
        {
            var items = Cache.Where(p => p.Key.Contains(register + ":"));
            foreach (var item in items)
            {
                Cache.Remove(item.Key);
            }
        }

        public T GetOrAdd<T>(string register, string key, Func<T> onCacheMissed)
        {
            var fullKey = GenerateKey(register, key);
            var result = Cache.Get(fullKey);
            if (result != null)
            {
                return (T)result;
            }

            lock (CacheLock)
            {
                // check again after lock.
                result = Cache.Get(fullKey);
                if (result != null)
                {
                    return (T)result;
                }

                result = onCacheMissed();
                Cache.Add(fullKey, result, new CacheItemPolicy());
            }
            
            return (T)result;
        }

        public bool TryGet<T>(string register, string key, out T element)
        {
            var item = Cache.Get(GenerateKey(register, key));
            if (item == null)
            {
                element = default(T);
                return false;
            }

            element = (T)item;
            return true;
        }

        private static string GenerateKey(string register, string key)
        {
            return register + ":" + key;
        }
    }
}
