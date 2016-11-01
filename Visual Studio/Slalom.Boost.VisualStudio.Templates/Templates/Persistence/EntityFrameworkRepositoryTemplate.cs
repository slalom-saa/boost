namespace Slalom.Boost.Templates
{
    public class EntityFrameworkRepositoryTemplate : Template
    {
        private static readonly string TemplateContent = Files.EntityFrameworkRepositoryTemplate;

        public EntityFrameworkRepositoryTemplate()
            : base("EntityFrameworkRepository", TemplateContent)
        {
        }
    }
}