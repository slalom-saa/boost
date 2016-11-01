namespace Slalom.Boost.Templates
{
    public class ReadModelTemplate : Template
    {
        private static readonly string TemplateContent = Files.ReadModelTemplate;

        public ReadModelTemplate()
            : base("ReadModel", TemplateContent)
        {
        }
    }
}