using System.Windows.Forms;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers
{
    public class AddAdminController : VisualStudioCommand
    {
        public AddAdminController()
            : base(0x0520)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay(ProjectItemDescriptor projectItem)
        {
            return projectItem.Project.IsWebApi || !new AdminController(projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
            var item = new AdminController(this.ProjectItem).Build();
            this.Complete("AdminController", item);
        }
    }
}