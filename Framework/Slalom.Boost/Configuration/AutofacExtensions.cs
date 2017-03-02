using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;

namespace Slalom.Boost.Configuration
{
    public static class AutofacExtensions
    {
        public static IEnumerable<T> ResolveAll<T>(this IComponentContext instance)
        {
            return instance.Resolve<IEnumerable<T>>();
        }

        public static IEnumerable<object> ResolveAll(this IComponentContext instance, Type type)
        {
            return (IEnumerable<object>)instance.Resolve(typeof(IEnumerable<>).MakeGenericType(type));
        }
    }
}
