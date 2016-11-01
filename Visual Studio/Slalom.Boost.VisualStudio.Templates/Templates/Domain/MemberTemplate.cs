namespace Slalom.Boost.Templates
{
    public class MemberTemplate : Template
    {
        private static readonly string TemplateContent = Files.MemberTemplate;

        public MemberTemplate()
            : base("Member", TemplateContent)
        {
        }
    }
}