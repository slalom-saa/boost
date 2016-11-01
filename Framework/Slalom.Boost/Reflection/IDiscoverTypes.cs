using System;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.Reflection
{
    /// <summary>
    /// Discovers types that exist in a given context.
    /// </summary>
    public interface IDiscoverTypes
    {
        /// <summary>
        /// Finds all types in the context.
        /// </summary>
        /// <typeparam name="TType">The type's base class or interface.</typeparam>
        /// <returns>Returns all types in the context.</returns>
        IEnumerable<Type> Find<TType>();
    }
}