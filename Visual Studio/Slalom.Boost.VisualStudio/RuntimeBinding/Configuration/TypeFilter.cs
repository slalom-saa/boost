using System;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class TypeFilter : BindingFilter
    {
        private readonly Func<Type, bool> _filter;

        private TypeFilter(Func<Type, bool> filter)
        {
            _filter = filter;
        }

        public static TypeFilter Exclude(params Type[] types)
        {
            return new TypeFilter(e => types.Contains(e));
        }

        public static TypeFilter Only(params Type[] types)
        {
            return new TypeFilter(e => !types.Contains(e));
        }

        public static TypeFilter Exclude(Func<Type, bool> predicate)
        {
            return new TypeFilter(e => predicate(e));
        }

        public bool Filter(Type assembly)
        {
            return _filter(assembly);
        }

        public IEnumerable<TypeFilter> AsEnumerable()
        {
            return new[] { this };
        }
    }
}