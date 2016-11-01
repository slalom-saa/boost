using System.IO;
using System.Linq;
using System.Windows.Forms;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddProcessManagerCommand : VisualStudioCommand
    {
        public AddProcessManagerCommand()
            : base(0x0512)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay()
        {
            return false;
            //turn projectItem.IsProcessManager;
        }

        protected override void HandleCallback()
        {
            //using (var form = new AddItemForm("Enter Command Name (AddSomething, AcceptSomething)"))
            //{
            //    if (form.ShowDialog() == DialogResult.OK)
            //    {
            //        new ProcessManagerCommandBuilder(Path.GetFileName(Path.GetDirectoryName(this.ProjectItem.Path)).Singularize(), form.ItemName, form.Properties, this.ProjectItem).Build();
            //    }
            //}
        }
    }
}