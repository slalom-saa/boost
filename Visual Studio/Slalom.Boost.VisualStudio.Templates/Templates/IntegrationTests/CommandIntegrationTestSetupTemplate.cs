using System;

namespace Slalom.Boost.Templates.Templates.IntegrationTests
{
    public class CommandIntegrationTestSetupTemplate : Template
    {
        private static readonly string TemplateContent = Files.CommandIntegrationTestSetupTemplate;

        public CommandIntegrationTestSetupTemplate()
            : base("CommandIntegrationTestSetup", TemplateContent)
        {
        }
    }
}