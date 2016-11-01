using System;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence
{
    public class AddEntityFrameworkRepository : VisualStudioCommand
    {
        public AddEntityFrameworkRepository()
            : base(0x0510)
        {
        }

        public override bool ShouldDisplay()
        {
            //var project = GetPersistenceProject(projectItem);

            //return projectItem.IsAggregateRoot && !new EntityFrameworkRepositoryBuilder(projectItem).FileExists(project);

            return false;
        }

        protected override void HandleCallback()
        {
            //var project = EnsurePersistenceProject(this.ProjectItem, "Product.Project.Persistence");

            ////new DataObjectBuilder(this.ProjectItem).Build(project);
            //new EntityMappingBuilder(this.ProjectItem).Build(project);
            //new EntityFrameworkRepositoryBuilder(this.ProjectItem).Build(project);
        }
    }
}