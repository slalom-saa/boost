using System;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence
{
    public class AddEntityFrameworkProject : VisualStudioCommand
    {
        public AddEntityFrameworkProject()
            : base(0x0409)
        {
        }

        public override bool ShouldDisplay()
        {
            //return projectItem.IsProject && GetPersistenceProject(projectItem) == null;
            return false;
        }

        protected override void HandleCallback()
        {
           // EnsurePersistenceProject(this.ProjectItem, "Product.Project.Persistence", GetApplicationProject(this.ProjectItem), GetDomainProject(this.ProjectItem));
        }
    }
}