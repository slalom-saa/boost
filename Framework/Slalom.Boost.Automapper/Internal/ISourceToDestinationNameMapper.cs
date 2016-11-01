using System;
using System.Reflection;

namespace Slalom.Boost.AutoMapper.Internal
{
    public interface ISourceToDestinationNameMapper
    {
        MemberInfo GetMatchingMemberInfo(IGetTypeInfoMembers getTypeInfoMembers, TypeDetails typeInfo, Type destType, string nameToSearch);
    }
}