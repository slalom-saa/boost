using System;

namespace Slalom.Boost.AutoMapper.Internal
{
    public interface IEnumNameValueMapper
    {
        bool IsMatch(Type enumDestinationType, string sourceValue);
        object Convert(Type enumSourceType, Type enumDestinationType, ResolutionContext context);
    }
}