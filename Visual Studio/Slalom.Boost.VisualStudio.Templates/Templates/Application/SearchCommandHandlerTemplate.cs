namespace Slalom.Boost.Templates
{
    public class SearchCommandHandlerTemplate : Template
    {
        private static readonly string TemplateContent = Files.SearchCommandHandlerTemplate;

        public SearchCommandHandlerTemplate()
            : base("SearchCommandHandler", TemplateContent)
        {
        }
    }
}