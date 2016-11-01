namespace Slalom.Boost.AutoMapper.Mappers
{
    public interface ITypeMapObjectMapper
    {
        object Map(ResolutionContext context);
        bool IsMatch(ResolutionContext context);
    }
}