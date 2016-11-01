namespace Slalom.Boost.Templates
{
    public class AggregateAddedEventTemplate : Template
    {
        private static readonly string TemplateContent = Files.AggregateAddedEventTemplate;

        public AggregateAddedEventTemplate()
            : base("AggregateAddedEvent", TemplateContent)
        {
        }
    }
}