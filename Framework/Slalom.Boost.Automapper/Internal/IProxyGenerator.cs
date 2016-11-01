using System;

namespace Slalom.Boost.AutoMapper.Internal
{
    public interface IProxyGenerator
    {
        Type GetProxyType(Type interfaceType);
    }
}