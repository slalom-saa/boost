using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddBusinessValidationRuleCommand : VisualStudioCommand
    {
        public AddBusinessValidationRuleCommand()
            : base(0x0403)
        {
            this.Markers.Add("// TODO: Add any business rules [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.ProjectItem.IsCommand()
                   && !this.Solution.GetApplicationProject().FileExists(this.ProjectItem.GetRelativeFolder() + "\\Rules\\business_rule.cs");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var rule = this.AddFile("BusinessValidationRule", item.GetRelativeFolder() + "\\Rules\\business_rule.cs", this.Solution.GetApplicationProject(), item);

            this.WriteOutput(rule);

            return rule;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}