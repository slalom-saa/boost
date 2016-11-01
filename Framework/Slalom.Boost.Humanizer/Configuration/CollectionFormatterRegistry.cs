using Slalom.Boost.Humanizer.Localisation.CollectionFormatters;

namespace Slalom.Boost.Humanizer.Configuration
{
    internal class CollectionFormatterRegistry : LocaliserRegistry<ICollectionFormatter>
    {
        public CollectionFormatterRegistry()
            : base(new DefaultCollectionFormatter("&"))
        {
            this.Register("en", new OxfordStyleCollectionFormatter("and"));
            this.Register("it", new DefaultCollectionFormatter("e"));
            this.Register("de", new DefaultCollectionFormatter("und"));
            this.Register("dk", new DefaultCollectionFormatter("og"));
            this.Register("nl", new DefaultCollectionFormatter("en"));
            this.Register("pt", new DefaultCollectionFormatter("e"));
            this.Register("ro", new DefaultCollectionFormatter("și"));
            this.Register("nn", new DefaultCollectionFormatter("og"));
            this.Register("nb", new DefaultCollectionFormatter("og"));
        }
    }
}