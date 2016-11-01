using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class PowerShellCommandTemplate : Template
    {
        private static readonly string TemplateContent = Files.PowerShellCommandTemplate;

        public PowerShellCommandTemplate()
            : base("PowerShellCommand", TemplateContent)
        {
        }

        public override string Build(string name, Project project, ProjectItem item, object replacements = null)
        {
            return base.Build(name, project, item).ReplaceTokens(new
            {
                ParametersSection = string.Join("", this.GetParameterSection(item)),
                CommandArguments = string.Join("", this.GetCommandArguments(item))
            });
        }

        private IEnumerable<string> GetCommandArguments(ProjectItem item)
        {
            var parameters = item.GetCodeProperties().ToList();
            foreach (var parameter in parameters)
            {
                yield return $"${parameter.Name.ToPascalCase()}";
                if (parameter != parameters.Last())
                {
                    yield return ", ";
                }
            }
        }

        private IEnumerable<string> GetParameterSection(ProjectItem item)
        {
            var parameters = item.GetCodeProperties().ToList();
            foreach (var parameter in parameters)
            {
                yield return "\t[Parameter(Mandatory = $true)]" + Environment.NewLine;
                yield return $"\t[{parameter.Type.AsFullName}]${parameter.Name.ToPascalCase()}";
                if (parameter != parameters.Last())
                {
                    yield return "," + Environment.NewLine + Environment.NewLine;
                }
            }
        }
    }
}