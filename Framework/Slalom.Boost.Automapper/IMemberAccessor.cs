namespace Slalom.Boost.AutoMapper
{
    public interface IMemberAccessor : IMemberGetter
    {
        void SetValue(object destination, object value);
    }
}