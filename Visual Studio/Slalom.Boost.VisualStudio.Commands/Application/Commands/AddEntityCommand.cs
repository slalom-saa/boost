using System.Windows.Forms;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddEntityCommand : VisualStudioCommand
    {
        public AddEntityCommand()
            : base(0x0515)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay()
        {
            return false;
            //return projectItem.IsEntity;
        }

        protected override void HandleCallback()
        {
        //    using (var form = new AddItemForm("Add Command"))
        //    {
        //        if (form.ShowDialog() == DialogResult.OK)
        //        {
        //            form.Activate();

        //            var project = GetApplicationProject(this.ProjectItem);

        //            var name = form.ItemName.EndsWith("Commmand") ? form.ItemName.Substring(0, form.ItemName.Length - 9) : form.ItemName;
        //            var command = new ProcessManagerCommandBuilder(this.ProjectItem.ClassName, name, form.Properties, this.ProjectItem).Build(project);
        //            var handler = new CommandHandlerBuilder(name, this.ProjectItem, command).Build(project);

        //            command.Open();

        //            this.Complete("Command", command, handler);
        //        }
        //    }
        }
    }
}