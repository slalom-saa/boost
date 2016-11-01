namespace Slalom.Boost.Templates
{
    public class SearchCommandTemplate : Template
    {
        private static readonly string TemplateContent = Files.SearchCommandTemplate;

        public SearchCommandTemplate()
            : base("SearchCommand", TemplateContent)
        {
        }
    }
}