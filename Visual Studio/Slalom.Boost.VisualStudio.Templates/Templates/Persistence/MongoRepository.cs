namespace Slalom.Boost.Templates
{
    public class MongoRepositoryTemplate : Template
    {
        private static readonly string TemplateContent = Files.MongoRepositoryTemplate;

        public MongoRepositoryTemplate()
            : base("MongoRepository", TemplateContent)
        {
        }
    }
}