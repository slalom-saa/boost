using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddInRoleValidationRuleCommand : VisualStudioCommand
    {
        public AddInRoleValidationRuleCommand()
            : base(0x0509)
        {
            this.Markers.Add("// TODO: Add any security rules [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.ProjectItem.IsCommand()
                   && !this.Solution.GetApplicationProject().FileExists(this.ProjectItem.GetRelativeFolder() + "\\Rules\\has_permissions.cs");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var rule = this.AddFile("InRoleValidationRule", item.GetRelativeFolder() + "\\Rules\\has_permissions.cs", this.Solution.GetApplicationProject(), item);

            this.WriteOutput(rule);

            return rule;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}