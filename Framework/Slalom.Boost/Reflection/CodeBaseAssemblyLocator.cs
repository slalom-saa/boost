using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Slalom.Boost.Reflection
{
    /// <summary>
    /// Locates assemblies using the current code base.
    /// </summary>
    public class CodeBaseAssemblyLocator : IAssemblyLocator
    {
        /// <summary>
        /// Locates and returns an observable collection of assemblies.
        /// </summary>
        /// <returns>Returns an observable collection of assemblies</returns>
        public ObservableCollection<_Assembly> Locate()
        {
            var codeBase = typeof(CodeBaseAssemblyLocator).Assembly.GetName().CodeBase;

            var uri = new Uri(codeBase);

            var fileInfo = new FileInfo(uri.LocalPath);

            var files = Directory.GetFiles(fileInfo.Directory.ToString(), "*.dll").ToList();

            var assemblies = files.Where(e => !e.Contains("Microsoft.Azure") && !e.Contains("DocumentDb")).Select(Assembly.LoadFrom).OfType<_Assembly>();

            return new ObservableCollection<_Assembly>(assemblies);
        }
    }
}