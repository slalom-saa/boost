using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Defines an inclusion filter for assemblies to be loaded into the runtime binding process.
    /// </summary>
    /// <seealso cref="Slalom.Boost.RuntimeBinding.Configuration.BindingFilter" />
    public class AssemblyFilter : BindingFilter
    {
        private readonly Func<_Assembly, bool> _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssemblyFilter"/> class.
        /// </summary>
        /// <param name="filter">The assembly filter.</param>
        private AssemblyFilter(Func<_Assembly, bool> filter)
        {
            _filter = filter;
        }

        /// <summary>
        /// Executes the filter to see if the assembly should be included.
        /// </summary>
        /// <param name="assembly">The assembly to check.</param>
        /// <returns><c>true</c> if the assembly should be included, <c>false</c> otherwise.</returns>
        public bool Filter(_Assembly assembly)
        {
            return _filter(assembly);
        }

        /// <summary>
        /// Includes assemblies that match the specified filter.
        /// </summary>
        /// <param name="filter">The filter.</param>
        /// <returns>Returns the created instance.</returns>
        public static AssemblyFilter Include(Func<_Assembly, bool> filter)
        {
            return new AssemblyFilter(filter);
        }

        /// <summary>
        /// Includes all specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to include.</param>
        /// <returns>Returns the created filter.</returns>
        public static AssemblyFilter Include(params _Assembly[] assemblies)
        {
            return new AssemblyFilter(e => assemblies.Any(x => e.FullName == x.FullName));
        }
    }
}