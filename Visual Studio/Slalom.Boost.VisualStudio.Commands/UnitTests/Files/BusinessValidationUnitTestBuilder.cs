using System;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class BusinessValidationUnitTestBuilder : CommandBasedClassBuilder
    {
        public BusinessValidationUnitTestBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.BusinessValidationUnitTest, selectedItem)
        {
            this.RelativePath = $"Application\\Commands\\{this.Name}\\When_validating_{this.Rule}_for_a_{this.NameSnakeCase}_command.cs";
        }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"BusinessValidationRule<(\S*)Command>").Groups[1].Value;
        }
    }
}