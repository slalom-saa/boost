namespace Slalom.Boost.Templates
{
    public class UserNameTemplate : Template
    {
        private static readonly string TemplateContent = Files.UserNameTemplate;

        public UserNameTemplate()
            : base("UserName", TemplateContent)
        {
        }
    }

    public class PasswordTemplate : Template
    {
        private static readonly string TemplateContent = Files.PasswordTemplate;

        public PasswordTemplate()
            : base("Password", TemplateContent)
        {
        }
    }

    public class StringConceptTemplate : Template
    {
        private static readonly string TemplateContent = Files.StringConceptTemplate;

        public StringConceptTemplate()
            : base("StringConcept", TemplateContent)
        {
        }
    }
}