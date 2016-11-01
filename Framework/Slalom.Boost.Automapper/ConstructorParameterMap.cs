using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.AutoMapper
{
    public class ConstructorParameterMap
    {
        public ConstructorParameterMap(ParameterInfo parameter, IValueResolver[] sourceResolvers, bool canResolve)
        {
            this.Parameter = parameter;
            this.SourceResolvers = sourceResolvers;
            this.CanResolve = canResolve;
        }

        public ParameterInfo Parameter { get; private set; }

        public IValueResolver[] SourceResolvers { get; private set; }

        public bool CanResolve { get; set; }

        public ResolutionResult ResolveValue(ResolutionContext context)
        {
            var result = new ResolutionResult(context);

            return this.SourceResolvers.Aggregate(result, (current, resolver) => resolver.Resolve(current));
        }

        public void ResolveUsing(IEnumerable<IMemberGetter> members)
        {
            this.SourceResolvers = members.ToArray();
        }
    }
}