using System;
using System.IO;
using System.Text.RegularExpressions;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class InputValidationUnitTestBuilder : CommandBasedClassBuilder
    {
        public InputValidationUnitTestBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.InputValidationUnitTest, selectedItem)
        {
            this.Rule = Path.GetFileNameWithoutExtension(selectedItem.Path).ToSnakeCase();

            this.RelativePath = $"Application\\Commands\\{this.Name}\\When_validating_{this.NameSnakeCase}_command_input_with_invalid_input.cs";

            this.Namespace = selectedItem.ClassDescriptor.Namespace;
        }

        public string Namespace { get; set; }

        public new string Rule { get; set; }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"InputValidationRuleSet<(\S*)Command>", RegexOptions.Multiline).Groups[1].Value;
        }
    }
}