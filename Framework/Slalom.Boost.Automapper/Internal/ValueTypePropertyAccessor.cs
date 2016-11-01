using System.Reflection;

namespace Slalom.Boost.AutoMapper.Internal
{
    public class ValueTypePropertyAccessor : PropertyGetter, IMemberAccessor
    {
        private readonly MethodInfo _lateBoundPropertySet;

        public ValueTypePropertyAccessor(PropertyInfo propertyInfo)
            : base(propertyInfo)
        {
            var method = propertyInfo.GetSetMethod(true);
            this.HasSetter = method != null;
            if (this.HasSetter)
            {
                _lateBoundPropertySet = method;
            }
        }

        public bool HasSetter { get; }

        public void SetValue(object destination, object value)
        {
            _lateBoundPropertySet.Invoke(destination, new[] {value});
        }
    }
}