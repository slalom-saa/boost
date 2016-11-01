using System;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Automation.Files
{
    public class PowerShellCommand : ClassBuilder
    {
        public PowerShellCommand(ProjectItemDescriptor selectedItem)
            : base(Templates.PowerShellCommand, selectedItem)
        {
            this.RelativePath = @"Scripts\Commands\" + selectedItem.CommandName + ".ps1";
        }

        public string ParametersSection => string.Concat(this.GetParameterSection());

        public string CommandArguments => string.Concat(this.GetCommandArguments());

        public string Command => this.ProjectItem.ClassDescriptor.Namespace + "." + this.ProjectItem.ClassName;

        private IEnumerable<string> GetCommandArguments()
        {
            var parameters = this.GetConstructorParameters().ToList();
            foreach (var parameter in parameters)
            {
                yield return $"${parameter.Name.ToPascalCase()}";
                if (parameter != parameters.Last())
                {
                    yield return ", ";
                }
            }
        }

        private IEnumerable<Parameter> GetConstructorParameters()
        {
            return ProjectItem.ClassDescriptor.GetConstructorParameters();
        }

        private IEnumerable<string> GetParameterSection()
        {
            var parameters = this.GetConstructorParameters().ToList();
            foreach (var parameter in parameters)
            {
                yield return "\t[Parameter(Mandatory = $true)]" + Environment.NewLine;
                yield return $"\t[{parameter.ParameterType}]${parameter.Name.ToPascalCase()}";
                if (parameter != parameters.Last())
                {
                    yield return "," + Environment.NewLine + Environment.NewLine;
                }
            }
        }
    }
}