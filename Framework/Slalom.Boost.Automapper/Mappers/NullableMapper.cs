using Slalom.Boost.AutoMapper.Internal;

namespace Slalom.Boost.AutoMapper.Mappers
{
    public class NullableMapper : IObjectMapper
    {
        public object Map(ResolutionContext context)
        {
            return context.SourceValue;
        }

        public bool IsMatch(TypePair context)
        {
            return context.DestinationType.IsNullableType();
        }
    }
}