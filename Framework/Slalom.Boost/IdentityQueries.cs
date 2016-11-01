using System;
using System.Linq;

namespace Slalom.Boost
{
    public static class IdentityQueries
    {
        public static IQueryable<T> ById<T>(this IQueryable<T> instance, Guid id) where T : IHaveIdentity
        {
            return instance.Where(e => e.Id == id);
        }

        public static bool Exists<T>(this IQueryable<T> instance, Guid id) where T : IHaveIdentity
        {
            return instance.Any(e => e.Id == id);
        }
    }
}