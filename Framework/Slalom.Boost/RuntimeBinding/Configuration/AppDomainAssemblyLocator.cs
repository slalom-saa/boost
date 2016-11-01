using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Locates assemblies using the current application domain.
    /// </summary>
    /// <seealso cref="Slalom.Boost.RuntimeBinding.Configuration.IAssemblyLocator" />
    public class AppDomainAssemblyLocator : IAssemblyLocator
    {
        private readonly ObservableCollection<_Assembly> _collection = new ObservableCollection<_Assembly>();

        /// <summary>
        /// Locates and returns an observable collection of assemblies.
        /// </summary>
        /// <param name="filters">The assembly filters to use when locating assemblies.</param>
        /// <returns>Returns an observable collection of assemblies</returns>
        public ObservableCollection<_Assembly> Locate(params AssemblyFilter[] filters)
        {
            var located = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic && filters.Any(filter => filter.Filter(assembly)));
            new[] { typeof(AppDomainAssemblyLocator).Assembly }.Union(
                located).ToList().ForEach(e =>
                {
                    _collection.Add(e);
                });

            AppDomain.CurrentDomain.AssemblyLoad += this.HandleAssemblyLoaded;

            return _collection;
        }

        private void HandleAssemblyLoaded(object sender, AssemblyLoadEventArgs args)
        {
            _collection.Add(args.LoadedAssembly);
        }
    }
}