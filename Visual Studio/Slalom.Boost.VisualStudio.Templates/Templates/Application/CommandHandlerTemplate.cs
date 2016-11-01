namespace Slalom.Boost.Templates
{
    public class CommandHandlerTemplate : Template
    {
        private static readonly string TemplateContent = Files.CommandHandlerTemplate;

        public CommandHandlerTemplate()
            : base("CommandHandler", TemplateContent)
        {
        }
    }
}