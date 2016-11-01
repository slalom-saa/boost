using System.Reflection;

namespace Slalom.Boost.AutoMapper
{
    public interface IMemberGetter : IMemberResolver
    {
        MemberInfo MemberInfo { get; }
        string Name { get; }
        object GetValue(object source);
    }
}