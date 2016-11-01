using System;
using System.Reflection;

namespace Slalom.Boost.AutoMapper.Internal
{
    public class PropertyAccessor : PropertyGetter, IMemberAccessor
    {
        private readonly Lazy<LateBoundPropertySet> _lateBoundPropertySet;

        public PropertyAccessor(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            this.HasSetter = propertyInfo.GetSetMethod(true) != null;
            if (this.HasSetter)
            {
                _lateBoundPropertySet = new Lazy<LateBoundPropertySet>(() => DelegateFactory.CreateSet(propertyInfo));
            }
        }

        public bool HasSetter { get; }

        public virtual void SetValue(object destination, object value)
        {
            _lateBoundPropertySet.Value(destination, value);
        }
    }
}