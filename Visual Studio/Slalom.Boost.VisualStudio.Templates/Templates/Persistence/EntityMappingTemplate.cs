namespace Slalom.Boost.Templates
{
    public class EntityMappingTemplate : Template
    {
        private static readonly string TemplateContent = Files.EntityMappingTemplate;

        public EntityMappingTemplate()
            : base("EntityMapping", TemplateContent)
        {
        }
    }
}