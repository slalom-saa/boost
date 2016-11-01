namespace Slalom.Boost.Templates
{
    public class AggregateTemplate : Template
    {
        private static readonly string TemplateContent = Files.AggregateTemplate;

        public AggregateTemplate()
            : base("Aggregate", TemplateContent)
        {
        }
    }
}