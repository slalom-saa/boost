using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddBusinessValidationRule : VisualStudioCommand
    {
        public AddBusinessValidationRule()
            : base(0x0403)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsFile && projectItem.IsCommand;
        }

        protected override void HandleCallback()
        {
            //int index = 1;
            //var builder = new BusinessValidationRuleBuilder(this.ProjectItem.CommandName, index, this.ProjectItem);
            //while (builder.FileExists())
            //{
            //    index++;
            //    builder = new BusinessValidationRuleBuilder(this.ProjectItem.CommandName, index, this.ProjectItem);
            //}
            //builder.Build();
        }
    }
}