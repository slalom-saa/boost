using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands
{
    public class AddCommandCommand : VisualStudioCommand
    {
        public AddCommandCommand()
            : base(0x0515)
        {
            this.Markers.Add("// TODO: Add any additional commands [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.SelectedItem.ProjectItem.IsEntity();
        }



        public ProjectItem Execute(ProjectItem projectItem)
        {
            var form = new AddCommandWindow("Add Command", new[] { new BoostCodeProperty(projectItem.GetClassName() + "Id", typeof(Guid)) }.Concat(projectItem.GetCodeProperties()));
            {
                if (form.ShowDialog() == true)
                {
                    var entity = projectItem.GetClassName();
                    var name = form.ItemName.EndsWith("Commmand") ? form.ItemName.Substring(0, form.ItemName.Length - 9) : form.ItemName;
                    var properties = form.GetProperties();

                    var @event = this.AddFile("Event", $"{entity.Pluralize()}\\Events\\{form.EventName}.cs", form.EventName, this.Solution.GetDomainProject(), projectItem);

                    var command = this.AddFile("Command", $"{entity.Pluralize()}\\{name}\\{name}Command.cs", $"{name}", this.Solution.GetApplicationProject(), projectItem, properties, new
                    {
                        ReturnType = form.EventName
                    });

                    var handler = this.AddFile("CommandHandler", $"{entity.Pluralize()}\\{name}\\{name}CommandHandler.cs", $"{name}", this.Solution.GetApplicationProject(), projectItem, properties, new
                    {
                        ReturnType = form.EventName
                    });

                    this.WriteOutput(command, handler, @event);

                    return handler;
                }
            }
            return null;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}