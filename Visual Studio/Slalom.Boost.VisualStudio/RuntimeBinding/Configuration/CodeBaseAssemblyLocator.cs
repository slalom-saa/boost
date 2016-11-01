using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class CodeBaseAssemblyLocator : IAssemblyLocator
    {
        public ObservableCollection<_Assembly> Locate(AssemblyFilter[] filters)
        {
            var codeBase = typeof(CodeBaseAssemblyLocator).Assembly.GetName().CodeBase;

            var uri = new Uri(codeBase);

            var fileInfo = new FileInfo(uri.LocalPath);

            var files = Directory.GetFiles(fileInfo.Directory.ToString(), "*.dll").ToList();

            var assemblies = files.Select(Assembly.LoadFrom).OfType<_Assembly>();
            
            return new ObservableCollection<_Assembly>(assemblies.Where(assembly => filters.Any(filter => filter.Filter(assembly))));
        }
    }
}