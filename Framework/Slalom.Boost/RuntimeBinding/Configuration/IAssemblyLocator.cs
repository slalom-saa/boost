using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Defines a contract for locating assemblies.
    /// </summary>
    [IgnoreBinding]
    internal interface IAssemblyLocator
    {
        /// <summary>
        /// Locates and returns an observable collection of assemblies.
        /// </summary>
        /// <param name="filters">The assembly filters to use when locating assemblies.</param>
        /// <returns>Returns an observable collection of assemblies</returns>
        ObservableCollection<_Assembly> Locate(AssemblyFilter[] filters);
    }
}