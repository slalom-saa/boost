using System;

namespace Slalom.Boost.AutoMapper
{
    public interface IMemberResolver : IValueResolver
    {
        Type MemberType { get; }
    }
}