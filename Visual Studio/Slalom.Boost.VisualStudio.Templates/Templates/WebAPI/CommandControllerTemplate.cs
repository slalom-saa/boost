namespace Slalom.Boost.Templates
{
    public class CommandControllerTemplate : Template
    {
        private static readonly string TemplateContent = Files.CommandControllerTemplate;

        public CommandControllerTemplate()
            : base("CommandController", TemplateContent)
        {
        }
    }
}