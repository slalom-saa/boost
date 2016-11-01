using System;

namespace Slalom.Boost.Templates.Templates.IntegrationTests
{
    public class CommandIntegrationTestTemplate : Template
    {
        private static readonly string TemplateContent = Files.CommandIntegrationTestTemplate;

        public CommandIntegrationTestTemplate()
            : base("CommandIntegrationTest", TemplateContent)
        {
        }
    }
}