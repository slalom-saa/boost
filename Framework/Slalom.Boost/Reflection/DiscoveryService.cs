using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Slalom.Boost.Reflection
{
    /// <summary>
    /// Scans and locates types and assemblies given the current context.
    /// </summary>
    /// <seealso cref="IDiscoverTypes" />
    public class DiscoveryService : IDiscoverTypes
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DiscoveryService"/> class.
        /// </summary>
        public DiscoveryService(_Assembly[] assemblies)
        {
            this.Assemblies = assemblies;
        }

        public _Assembly[] Assemblies { get; set; }

        /// <summary>
        /// Finds available types that are assignable to the specified type.
        /// </summary>
        /// <typeparam name="TType">The type that found types are assignable to.</typeparam>
        /// <returns>All available types that are assignable to the specified type.</returns>
        public IEnumerable<Type> Find<TType>()
        {
            return this.Assemblies.SafelyGetTypes<TType>();
        }
    }
}