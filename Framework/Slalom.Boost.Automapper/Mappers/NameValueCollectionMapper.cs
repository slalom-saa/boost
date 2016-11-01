using System.Collections.Specialized;

#if !PORTABLE
namespace Slalom.Boost.AutoMapper.Mappers
{
    public class NameValueCollectionMapper : IObjectMapper
    {
        public object Map(ResolutionContext context)
        {
            if (context.SourceValue == null)
                return null;

            var nvc = new NameValueCollection();
            var source = (NameValueCollection)context.SourceValue;
            foreach (var s in source.AllKeys)
                nvc.Add(s, source[s]);

            return nvc;
        }

        public bool IsMatch(TypePair context)
        {
            return
                context.SourceType == typeof (NameValueCollection) &&
                context.DestinationType == typeof (NameValueCollection);
        }
    }
}
#endif