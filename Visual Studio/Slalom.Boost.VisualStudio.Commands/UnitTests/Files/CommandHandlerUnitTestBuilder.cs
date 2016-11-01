using System;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class CommandHandlerUnitTestBuilder : CommandBasedClassBuilder
    {
        public CommandHandlerUnitTestBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.CommandHandlerUnitTest, selectedItem)
        {
            var ns = selectedItem.ClassDescriptor.Namespace;
            this.RelativeNamespace = ns.Substring(ns.IndexOf(".Application"));
            this.RelativePath = $"Application\\Commands\\{this.Name}\\When_handling_a_{this.NameSnakeCase}_command.cs";
        }

        public string RelativeNamespace { get; set; }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"CommandHandler<(\S*)Command>").Groups[1].Value;
        }
    }
}