using System.Reflection;

namespace Slalom.Boost.AutoMapper.Internal
{
    public class ValueTypeFieldAccessor : FieldGetter, IMemberAccessor
    {
        private readonly FieldInfo _lateBoundFieldSet;

        public ValueTypeFieldAccessor(FieldInfo fieldInfo)
            : base(fieldInfo)
        {
            _lateBoundFieldSet = fieldInfo;
        }

        public void SetValue(object destination, object value)
        {
            _lateBoundFieldSet.SetValue(destination, value);
        }
    }
}