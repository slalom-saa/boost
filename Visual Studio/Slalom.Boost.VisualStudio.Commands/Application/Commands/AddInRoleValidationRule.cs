using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddInRoleValidationRule : VisualStudioCommand
    {
        public AddInRoleValidationRule()
            : base(0x0509)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsCommand
            //       && !new InRoleValidationRuleBuilder(projectItem.CommandName, projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
           // new InRoleValidationRuleBuilder(this.ProjectItem.CommandName, this.ProjectItem).Build();
        }
    }
}