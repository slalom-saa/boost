using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddEntityFrameworkMapping : VisualStudioCommand
    {
        public AddEntityFrameworkMapping()
            : base(0x0508)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //var project = GetPersistenceProject(projectItem);

            //return projectItem.IsReadModel && !new ReadModelMapping(projectItem).FileExists(project);
        }

        protected override void HandleCallback()
        {
            //var project = GetPersistenceProject(this.ProjectItem);

            //new ReadModelMapping(this.ProjectItem).Build(project);
        }
    }
}