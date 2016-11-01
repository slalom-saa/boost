using System.Diagnostics;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files;
using Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddReadModelQueryCommand : VisualStudioCommand
    {
        public AddReadModelQueryCommand()
            : base(0x0516)
        {
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsReadModel && !new QueryCommand(projectItem).FileExists();
        }

        protected override void HandleCallback()
        {
            //var item = new QueryCommand(this.ProjectItem).Build();
            //var handler = new QueryCommandHandlerBuilder("Query" + this.ProjectItem.ClassName.Replace("ReadModel", "").Pluralize(), this.ProjectItem.ClassName, item, item).Build();

            //Trace.WriteLine("");
            //Trace.WriteLine("2 files created");
            //Trace.WriteLine($"\t{item.Path}");
            //Trace.WriteLine($"\t{handler.Path}");
            //Trace.WriteLine("");

            //LocalEventPublisher.Publish("ContentChanged", "Command");
        }
    }
}