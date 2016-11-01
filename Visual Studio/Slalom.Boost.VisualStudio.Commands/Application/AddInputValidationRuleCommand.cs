using System.Linq;
using System.Text;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddInputValidationRuleCommand : VisualStudioCommand
    {
        public AddInputValidationRuleCommand()
            : base(0x0406)
        {
            this.Markers.Add("// TODO: Add an input validation rule [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.ProjectItem.IsCommand()
                && !this.Solution.GetApplicationProject().ClassExists(this.ProjectItem.GetClassName().ToSnakeCase() + "_is_valid");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var rules = new StringBuilder();
            foreach (var codeProperty in item.GetCodeProperties())
            {
                if (codeProperty.Type.AsString.Equals("string", System.StringComparison.OrdinalIgnoreCase))
                {
                    rules.AppendLine($"\t\t\tthis.Add(e => e.{codeProperty.Name})");
                    rules.AppendLine($"\t\t\t\t.NotNullOrWhiteSpace(\"{codeProperty.Name.Humanize(LetterCasing.Sentence)} cannot be null or whitespace.\");");
                    rules.AppendLine();
                }
            }

            var rule = this.AddFile("InputValidationRuleSet", item.GetRelativeFolder() + "\\Rules\\" + item.GetClassName().ToSnakeCase() + "_is_valid.cs", item.GetClassName().ToSnakeCase() + "_is_valid", this.Solution.GetApplicationProject(), item, null, new
            {
                Rules = rules
            });

            this.WriteOutput(rule);

            return rule;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}