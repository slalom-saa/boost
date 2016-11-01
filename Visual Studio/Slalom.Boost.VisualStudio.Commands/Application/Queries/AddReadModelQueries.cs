using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddReadModelQueries : VisualStudioCommand
    {
        public AddReadModelQueries()
            : base(0x0517)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsReadModel && !new ReadModelQueriesBuilder(projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
            //new ReadModelQueriesBuilder(this.ProjectItem).Build();
        }
    }
}