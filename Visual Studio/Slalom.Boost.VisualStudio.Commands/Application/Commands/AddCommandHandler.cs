using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddCommandHandler: VisualStudioCommand
    {
        public AddCommandHandler()
            : base(0x0405)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsFile && projectItem.IsCommand && !new CommandHandlerBuilder(projectItem.CommandName, projectItem, projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
            //new CommandHandlerBuilder(this.ProjectItem.CommandName, this.ProjectItem, this.ProjectItem).Build();
        }
    }
}