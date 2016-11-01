using System;
using System.Linq;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Represents an exclusionary filter for types to be excluded from runtime binding.
    /// </summary>
    /// <seealso cref="Slalom.Boost.RuntimeBinding.Configuration.BindingFilter" />
    public class TypeFilter : BindingFilter
    {
        private readonly Func<Type, bool> _filter;

        private TypeFilter(Func<Type, bool> filter)
        {
            _filter = filter;
        }

        /// <summary>
        /// Excludes the specified types.
        /// </summary>
        /// <param name="types">The types to exclude.</param>
        /// <returns>Returns a filter that will exclude types from runtime binding.</returns>
        public static TypeFilter Exclude(params Type[] types)
        {
            return new TypeFilter(types.Contains);
        }

        /// <summary>
        /// Excludes types that match the specified predicate.
        /// </summary>
        /// <param name="predicate">The predicate to use when determining if a type should be excluded.</param>
        /// <returns>Returns a filter that will exclude types from runtime binding.</returns>
        public static TypeFilter Exclude(Func<Type, bool> predicate)
        {
            return new TypeFilter(predicate);
        }

        /// <summary>
        /// Executes the filter and returns a value indicating whether the type should be excluded.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the type should be excluded, <c>false</c> otherwise.</returns>
        public bool Filter(Type type)
        {
            return _filter(type);
        }
    }
}