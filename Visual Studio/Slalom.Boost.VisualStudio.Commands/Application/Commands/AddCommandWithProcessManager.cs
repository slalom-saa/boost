using System.Linq;
using System.Windows.Forms;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddCommandWithProcessManager : VisualStudioCommand
    {
        public AddCommandWithProcessManager()
            : base(0x0511)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //var project = GetApplicationProject(projectItem);

            //return (projectItem.IsAggregateRoot) && !new ProcessManagerBuilder(projectItem.ClassName, projectItem).FileExists(project);
        }

        protected override void HandleCallback()
        {
            //var project = GetApplicationProject(this.ProjectItem);

            //new ProcessManagerBuilder(this.ProjectItem.ClassName, this.ProjectItem).Build(project);
        }
    }
}