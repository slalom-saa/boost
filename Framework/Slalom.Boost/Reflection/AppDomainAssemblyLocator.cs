using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.Reflection
{
    /// <summary>
    /// Locates assemblies using the current application domain.
    /// </summary>
    public class AppDomainAssemblyLocator : IAssemblyLocator
    {
        private readonly ObservableCollection<_Assembly> _collection = new ObservableCollection<_Assembly>();

        /// <summary>
        /// Locates and returns an observable collection of assemblies.
        /// </summary>
        /// <returns>Returns an observable collection of assemblies</returns>
        public ObservableCollection<_Assembly> Locate()
        {
            var located = AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic);
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