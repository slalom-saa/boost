using System;
using System.Collections.Generic;
using System.Reflection;

namespace Slalom.Boost.AutoMapper.Internal
{
    public abstract class MemberGetter : IMemberGetter
    {
        protected static readonly DelegateFactory DelegateFactory = new DelegateFactory();

        public abstract MemberInfo MemberInfo { get; }
        public abstract string Name { get; }
        public abstract Type MemberType { get; }
        public abstract object GetValue(object source);

        public ResolutionResult Resolve(ResolutionResult source)
        {
            return source.Value == null
                ? source.New(source.Value, this.MemberType)
                : source.New(this.GetValue(source.Value), this.MemberType);
        }

        public abstract IEnumerable<object> GetCustomAttributes(Type attributeType, bool inherit);
        public abstract IEnumerable<object> GetCustomAttributes(bool inherit);
        public abstract bool IsDefined(Type attributeType, bool inherit);
    }
}