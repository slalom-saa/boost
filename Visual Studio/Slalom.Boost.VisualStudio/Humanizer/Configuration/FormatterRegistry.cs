using Slalom.Boost.VisualStudio.Humanizer.Localisation.Formatters;

namespace Slalom.Boost.VisualStudio.Humanizer.Configuration
{
    internal class FormatterRegistry : LocaliserRegistry<IFormatter>
    {
        public FormatterRegistry() : base(new DefaultFormatter("en-US"))
        {
            this.Register("ar", new ArabicFormatter());
            this.Register("he", new HebrewFormatter());
            this.Register("ro", new RomanianFormatter());
            this.Register("ru", new RussianFormatter());
            this.Register("sl", new SlovenianFormatter());
            this.Register("hr", new CroatianFormatter());
            this.Register("sr", new SerbianFormatter("sr"));
            this.Register("sr-Latn", new SerbianFormatter("sr-Latn"));
            this.Register("uk", new UkrainianFormatter());
            this.RegisterCzechSlovakPolishFormatter("cs");
            this.RegisterCzechSlovakPolishFormatter("pl");
            this.RegisterCzechSlovakPolishFormatter("sk");
            this.RegisterDefaultFormatter("bg");
            this.RegisterDefaultFormatter("pt-BR");
            this.RegisterDefaultFormatter("sv");
            this.RegisterDefaultFormatter("tr");
            this.RegisterDefaultFormatter("vi");
            this.RegisterDefaultFormatter("en-US");
            this.RegisterDefaultFormatter("af");
            this.RegisterDefaultFormatter("da");
            this.RegisterDefaultFormatter("de");
            this.RegisterDefaultFormatter("el");
            this.RegisterDefaultFormatter("es");
            this.RegisterDefaultFormatter("fa");
            this.RegisterDefaultFormatter("fi-FI");
            this.RegisterDefaultFormatter("fr");
            this.RegisterDefaultFormatter("fr-BE");
            this.RegisterDefaultFormatter("hu");
            this.RegisterDefaultFormatter("id");
            this.RegisterDefaultFormatter("ja");
            this.RegisterDefaultFormatter("nb");
            this.RegisterDefaultFormatter("nb-NO");
            this.RegisterDefaultFormatter("nl");
            this.RegisterDefaultFormatter("bn-BD");
            this.RegisterDefaultFormatter("it");
            this.RegisterDefaultFormatter("uz-Latn-UZ");
            this.RegisterDefaultFormatter("uz-Cyrl-UZ");
            this.RegisterDefaultFormatter("zh-CN");
            this.RegisterDefaultFormatter("zh-Hans");
            this.RegisterDefaultFormatter("zh-Hant");
        }

        private void RegisterDefaultFormatter(string localeCode)
        {
            this.Register(localeCode, new DefaultFormatter(localeCode));
        }

        private void RegisterCzechSlovakPolishFormatter(string localeCode)
        {
            this.Register(localeCode, new CzechSlovakPolishFormatter(localeCode));
        }
    }
}
