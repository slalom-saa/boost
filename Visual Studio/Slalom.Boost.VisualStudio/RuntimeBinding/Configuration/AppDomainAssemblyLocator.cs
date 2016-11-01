using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    internal class AppDomainAssemblyLocator : IAssemblyLocator
    {
        private readonly ObservableCollection<_Assembly> _collection = new ObservableCollection<_Assembly>();

        public ObservableCollection<_Assembly> Locate(AssemblyFilter[] filters)
        {
                new[] { typeof(AppDomainAssemblyLocator).Assembly }.Union(
                    AppDomain.CurrentDomain.GetAssemblies().Where(assembly => !assembly.IsDynamic && filters.Any(filter => filter.Filter(assembly)))).ToList().ForEach(e =>
                    {
                        _collection.Add(e);
                    });

            AppDomain.CurrentDomain.AssemblyLoad += this.CurrentDomain_AssemblyLoad;

            return _collection;
        }

        private void CurrentDomain_AssemblyLoad(object sender, AssemblyLoadEventArgs args)
        {
            _collection.Add(args.LoadedAssembly);
        }
    }
}