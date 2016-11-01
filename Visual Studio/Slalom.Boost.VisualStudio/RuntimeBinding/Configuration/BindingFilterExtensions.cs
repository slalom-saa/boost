using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public static class BindingFilterExtensions
    {
        public static IEnumerable<Type> GetTypes(this IEnumerable<_Assembly> instance, IEnumerable<BindingFilter> filters)
        {
            var assemblies = instance.Where(e => filters.Filter(e));
            var allTypes = assemblies.SelectMany(e => e.SafelyGetTypes());
            var filtered = allTypes.Where(e => filters.Filter(e));
            return filtered;
        }

        public static IEnumerable<Type> Filter(this IEnumerable<Type> instance, IEnumerable<BindingFilter> filters)
        {
            return instance.Where(e => filters.Filter(e));
        }

        public static bool Filter(this IEnumerable<BindingFilter> filters, _Assembly assembly)
        {
            return filters.OfType<AssemblyFilter>().Any(e => e.Filter(assembly));
        }

        public static bool Filter(this IEnumerable<BindingFilter> filters, Type type)
        {
            return !type.ShouldIgnore() && filters.Filter(type.Assembly) && !filters.OfType<TypeFilter>().Any(e => e.Filter(type));
        }

        public static bool ShouldIgnore(this Type type)
        {
            return type.GetAllAttributes<IgnoreBindingAttribute>().Any();
        }
    }
}