using Slalom.Boost.VisualStudio.Humanizer.Localisation.NumberToWords;

namespace Slalom.Boost.VisualStudio.Humanizer.Configuration
{
    internal class NumberToWordsConverterRegistry : LocaliserRegistry<INumberToWordsConverter>
    {
        public NumberToWordsConverterRegistry()
            : base((culture) => new DefaultNumberToWordsConverter(culture))
        {
            this.Register("af", new AfrikaansNumberToWordsConverter());
            this.Register("en", new EnglishNumberToWordsConverter());
            this.Register("ar", new ArabicNumberToWordsConverter());
            this.Register("fa", new FarsiNumberToWordsConverter());
            this.Register("es", new SpanishNumberToWordsConverter());
            this.Register("pl", (culture) => new PolishNumberToWordsConverter(culture));
            this.Register("pt-BR", new BrazilianPortugueseNumberToWordsConverter());
            this.Register("ro", new RomanianNumberToWordsConverter());
            this.Register("ru", new RussianNumberToWordsConverter());
            this.Register("fi", new FinnishNumberToWordsConverter());
            this.Register("fr", new FrenchNumberToWordsConverter());
            this.Register("nl", new DutchNumberToWordsConverter());
            this.Register("he", (culture) => new HebrewNumberToWordsConverter(culture));
            this.Register("sl", (culture) => new SlovenianNumberToWordsConverter(culture));
            this.Register("de", new GermanNumberToWordsConverter());
            this.Register("bn-BD", new BanglaNumberToWordsConverter());
            this.Register("tr", new TurkishNumberToWordConverter());
            this.Register("it", new ItalianNumberToWordsConverter());
            this.Register("uk", new UkrainianNumberToWordsConverter());
            this.Register("uz-Latn-UZ", new UzbekLatnNumberToWordConverter());
            this.Register("uz-Cyrl-UZ", new UzbekCyrlNumberToWordConverter());
            this.Register("sr", (culture) => new SerbianCyrlNumberToWordsConverter(culture));
            this.Register("sr-Latn", (culture) => new SerbianNumberToWordsConverter(culture));
            this.Register("nb", new NorwegianBokmalNumberToWordsConverter());
        }
    }
}
