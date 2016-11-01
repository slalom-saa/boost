using System;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class BusinessValidationUnitTestSetupBuilder : CommandBasedClassBuilder
    {
        public BusinessValidationUnitTestSetupBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.BusinessValidationUnitTestSetup, selectedItem)
        {
            this.RelativePath = $"Application\\Commands\\{this.Name}\\given\\a_configured_{this.Rule}_rule.cs";
        }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"BusinessValidationRule<(\S*)Command>").Groups[1].Value;
        }
    }
}