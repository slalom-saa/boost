using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddAddAggregateCommandCommand : VisualStudioCommand
    {
        public AddAddAggregateCommandCommand()
            : base(0x0519)
        {
            this.Markers.Add("// TODO: Add an add command for creation [Boost]");
        }

        public ProjectItem Execute(ProjectItem projectItem)
        {
            var aggregate = projectItem.GetClassName();

            var command = this.AddFile("AddAggregateCommand", $"{aggregate.Pluralize()}\\Add{aggregate}\\Add{aggregate}Command.cs", this.Solution.GetApplicationProject(), projectItem);
            var handler = this.AddFile("AddAggregateCommandHandler", $"{aggregate.Pluralize()}\\Add{aggregate}\\Add{aggregate}CommandHandler.cs", this.Solution.GetApplicationProject(), projectItem);
            var @event = this.AddFile("AggregateAddedEvent", $"{aggregate.Pluralize()}\\Events\\{aggregate}AddedEvent.cs", this.Solution.GetDomainProject(), projectItem);

            this.WriteOutput(command, handler, @event);

            return handler;
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.SelectedItem.ProjectItem.IsEntity() &&
                   !this.Solution.GetApplicationProject().ClassExists($"Add{this.ProjectItem.GetClassName()}Command");
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}