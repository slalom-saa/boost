using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Locates assemblies using the current code base.
    /// </summary>
    /// <seealso cref="Slalom.Boost.RuntimeBinding.Configuration.IAssemblyLocator" />
    public class CodeBaseAssemblyLocator : IAssemblyLocator
    {
        /// <summary>
        /// Locates and returns an observable collection of assemblies.
        /// </summary>
        /// <param name="filters">The assembly filters to use when locating assemblies.</param>
        /// <returns>Returns an observable collection of assemblies</returns>
        public ObservableCollection<_Assembly> Locate(params AssemblyFilter[] filters)
        {
            var codeBase = typeof(CodeBaseAssemblyLocator).Assembly.GetName().CodeBase;

            var uri = new Uri(codeBase);

            var fileInfo = new FileInfo(uri.LocalPath);

            var files = Directory.GetFiles(fileInfo.Directory.ToString(), "*.dll").ToList();

            var assemblies = files.Where(e => !e.Contains("DocumentDB") && !e.Contains("Azure")).Select(Assembly.LoadFrom).OfType<_Assembly>();

            return new ObservableCollection<_Assembly>(assemblies.Where(assembly => filters.Any(filter => filter.Filter(assembly))));
        }
    }
}