using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddAddEntityCommand : VisualStudioCommand
    {
        public AddAddEntityCommand()
            : base(0x0519)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay()
        {
            var selected = new ProjectItemDescriptor(this.SelectedItem?.ProjectItem);
            return selected.Exists && selected.IsEntity && this.Solution.GetApplicationProject().ContainsFile($"Add{selected.ClassName}Command");
        }

        protected override void HandleCallback()
        {
            //var project = GetApplicationProject(this.ProjectItem);

            //var name = $"Add{this.ProjectItem.ClassName}";
            //var command = new ProcessManagerCommandBuilder(this.ProjectItem.ClassName, name,
            //    this.ProjectItem.ClassDescriptor.Properties.Where(e => e.Name != "Id"), this.ProjectItem).Build(project);
            //var handler = new AddCommandHandlerBuilder(name, this.ProjectItem, command).Build(project);

            //this.Complete("Command", command, handler);
        }
    }
}