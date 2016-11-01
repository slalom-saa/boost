namespace Slalom.Boost.Templates
{
    public class EventTemplate : Template
    {
        private static readonly string TemplateContent = Files.EventTemplate;

        public EventTemplate()
            : base("Event", TemplateContent)
        {
        }
    }
}