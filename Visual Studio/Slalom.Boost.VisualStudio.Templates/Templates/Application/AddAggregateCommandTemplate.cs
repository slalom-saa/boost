namespace Slalom.Boost.Templates
{
    public class AddAggregateCommandTemplate : Template
    {
        private static readonly string TemplateContent = Files.AddAggregateCommandTemplate;

        public AddAggregateCommandTemplate()
            : base("AddAggregateCommand", TemplateContent)
        {
        }
    }
}