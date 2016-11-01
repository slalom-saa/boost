using System;

namespace Slalom.Boost.AutoMapper
{
    public interface ITypeMapFactory
    {
        TypeMap CreateTypeMap(Type sourceType, Type destinationType, IProfileConfiguration mappingOptions, MemberList memberList);
    }
}