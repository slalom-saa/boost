using System;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests.Files
{
    public class CommandHandlerUnitTestSetupBuilder : CommandBasedClassBuilder
    {
        public CommandHandlerUnitTestSetupBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.CommandHandlerUnitTestSetup, selectedItem)
        {
            this.RelativePath = $"Application\\Commands\\{this.Name}\\given\\a_configured_{this.NameSnakeCase}_command_handler.cs";
        }

        protected override string GetCommandName(ProjectItemDescriptor selectedItem)
        {
            return Regex.Match(selectedItem.Content, @"CommandHandler<(\S*)Command>").Groups[1].Value;
        }
    }
}