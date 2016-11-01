using System;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class AssemblyFilter : BindingFilter
    {
        private readonly Func<_Assembly, bool> _filter;

        private AssemblyFilter(Func<_Assembly, bool> filter)
        {
            _filter = filter;
        }

        public static AssemblyFilter Include(Func<_Assembly, bool> filter)
        {
            return new AssemblyFilter(filter);
        }

        public static AssemblyFilter Include(params _Assembly[] assemblies)
        {
            return new AssemblyFilter(e => assemblies.Any(x => e.FullName == x.FullName));
        }

        public bool Filter(_Assembly assembly)
        {
            return _filter(assembly);
        }
    }
}