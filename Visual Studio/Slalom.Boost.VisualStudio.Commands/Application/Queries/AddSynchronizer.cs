using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddSynchronizer : VisualStudioCommand
    {
        public AddSynchronizer()
            : base(0x0505)
        {
        }

        public override bool ShouldDisplay()
        {
            //return projectItem.IsReadModel && !new Synchronizer(projectItem).FileExists();
            return false;
        }

        protected override void HandleCallback()
        {
            //var project = EnsurePersistenceProject(this.ProjectItem, "Product.Project.Persistence");

            //new ReadModelMapping(this.ProjectItem).Build(project);
            //new Synchronizer(this.ProjectItem).Build();
        }
    }
}