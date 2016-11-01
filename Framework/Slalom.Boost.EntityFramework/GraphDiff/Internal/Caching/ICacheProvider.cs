using System;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.Caching
{
    internal interface ICacheProvider
    {
        void Insert(string register, string key, object value);
        void Clear(string register);
        bool TryGet<T>(string register, string key, out T element);
        T GetOrAdd<T>(string register, string key, Func<T> onCacheMissed);
    }
}