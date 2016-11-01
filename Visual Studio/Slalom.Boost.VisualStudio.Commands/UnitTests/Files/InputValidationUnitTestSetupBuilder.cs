using System;
using System.IO;
using System.Text.RegularExpressions;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class InputValidationUnitTestSetupBuilder : CommandBasedClassBuilder
    {
        public InputValidationUnitTestSetupBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.InputValidationUnitTestSetup, selectedItem)
        {
            this.Rule = Path.GetFileNameWithoutExtension(selectedItem.Path).ToSnakeCase();

            this.RelativePath = $"Application\\Commands\\{this.Name}\\given\\a_configured_{this.Rule}_rule.cs";

            this.Namespace = selectedItem.ClassDescriptor.Namespace;
            this.UpperNamespace = this.Namespace.Substring(0, this.Namespace.LastIndexOf('.'));
            this.UpperNamespace = this.UpperNamespace.Substring(0, this.UpperNamespace.LastIndexOf('.'));
        }

        public string UpperNamespace { get; set; }

        public string Namespace { get; set; }

        public new string Rule { get; set; }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"InputValidationRuleSet<(\S*)Command>", RegexOptions.Multiline).Groups[1].Value;
        }
    }
}