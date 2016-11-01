namespace Slalom.Boost.Templates
{
    public class CommandTemplate : Template
    {
        private static readonly string TemplateContent = Files.CommandTemplate;

        public CommandTemplate()
            : base("Command", TemplateContent)
        {
        }
    }
}