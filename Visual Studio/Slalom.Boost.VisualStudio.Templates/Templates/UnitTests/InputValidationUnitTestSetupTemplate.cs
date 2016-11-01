using System;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class InputValidationUnitTestSetupTemplate : Template
    {
        private static readonly string TemplateContent = Files.InputValidationUnitTestSetupTemplate;

        public InputValidationUnitTestSetupTemplate()
            : base("InputValidationUnitTestSetup", TemplateContent)
        {
        }

        public override string Build(string name, Project project, ProjectItem item, object replacements = null)
        {
            return this.Content.ReplaceTokens(new
            {
                Project = project,
                Properties = item.GetCodeProperties(),
                ProjectItem = item,
                Name = name,
                Command = this.GetCommand(item)
            }, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));
        }

        private ProjectItem GetCommand(ProjectItem item)
        {
            var name = Regex.Match(item.GetContent(), @"class\W*\w*\W*:\W*InputValidationRuleSet<(\w*)>", RegexOptions.Multiline).Groups[1].Value;
            return item.ContainingProject.GetProjectItems().First(e => e.GetClassName() == name);
        }
    }
}