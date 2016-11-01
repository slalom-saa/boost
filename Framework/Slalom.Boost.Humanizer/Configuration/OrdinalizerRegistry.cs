using Slalom.Boost.Humanizer.Localisation.Ordinalizers;

namespace Slalom.Boost.Humanizer.Configuration
{
    internal class OrdinalizerRegistry : LocaliserRegistry<IOrdinalizer>
    {
        public OrdinalizerRegistry() : base(new DefaultOrdinalizer())
        {
            this.Register("de", new GermanOrdinalizer());
            this.Register("en", new EnglishOrdinalizer());
            this.Register("es", new SpanishOrdinalizer());
            this.Register("it", new ItalianOrdinalizer());
            this.Register("nl", new DutchOrdinalizer());
            this.Register("pt", new PortugueseOrdinalizer());
            this.Register("ro", new RomanianOrdinalizer());
            this.Register("ru", new RussianOrdinalizer());
            this.Register("tr", new TurkishOrdinalizer());
            this.Register("uk", new UkrainianOrdinalizer());
        }
    }
}
