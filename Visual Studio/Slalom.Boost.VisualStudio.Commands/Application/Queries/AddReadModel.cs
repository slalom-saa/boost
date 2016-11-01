using System.Diagnostics;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddReadModel : VisualStudioCommand
    {
        public AddReadModel()
            : base(0x0407)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //var project = GetApplicationProject(projectItem);

            //return (projectItem.IsEntity || projectItem.IsEventSource) && !new ReadModel(projectItem).FileExists(project);
        }

        protected override void HandleCallback()
        {
            //var project = GetApplicationProject(this.ProjectItem);

            //var item = new ReadModel(this.ProjectItem).Build(project);
            //var mapping = new ReadModelMapping(item).Build(EnsurePersistenceProject(this.ProjectItem, "Product.Project.Persistence"));
            //var sync = new Synchronizer(item).Build();

            //item.Open();

            //this.Complete("ReadModel", item, mapping, sync);
        }
    }
}