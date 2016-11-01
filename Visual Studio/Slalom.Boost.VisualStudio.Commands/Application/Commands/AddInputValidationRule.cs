using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddInputValidationRule : VisualStudioCommand
    {
        public AddInputValidationRule()
            : base(0x0406)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsCommand
            //       && !new InputValidationRuleSetBuilder(projectItem.CommandName, projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
            //new InputValidationRuleSetBuilder(this.ProjectItem.CommandName, this.ProjectItem).Build();
        }
    }
}