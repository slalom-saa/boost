using System.Windows.Forms;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers
{
    public class AddController : VisualStudioCommand
    {
        public AddController()
            : base(0x0501)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay(ProjectItemDescriptor projectItem)
        {
            return projectItem.Project.IsWebApi || (projectItem.IsEntity && !new CommandControllerBuilder(projectItem.ClassName, projectItem).FileExists(EnsureWebApiProject(projectItem, "Product.Project.WebApi")));
        }

        protected override void HandleCallback()
        {
            if (this.ProjectItem.IsEntity)
            {
                new CommandControllerBuilder(this.ProjectItem.ClassName, this.ProjectItem).Build(EnsureWebApiProject(this.ProjectItem, "Product.Project.WebApi"));
            }
            else
            {
                using (var form = new AddItemForm("Resource Name", false))
                {
                    if (form.ShowDialog() == DialogResult.OK)
                    {
                        new CommandControllerBuilder(form.ItemName, this.ProjectItem).Build();
                    }
                }
            }
        }
    }
}