using System.Linq;
using System.Windows.Forms;
using EnvDTE;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Commands.Domain
{
    public class AddValueObjectCommand : VisualStudioCommand
    {
        public AddValueObjectCommand()
            : base(0x0502)
        {
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.SelectedItem.IsProjectOrFolder() && this.SelectedItem.IsDomainItem();
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.Solution));
        }

        public ProjectItem Execute(Solution solution)
        {
            var form = new AddTemplateWindow("Add Value Object", "Generic", "UserName", "Password");
            {
                if (form.ShowDialog() == true)
                {
                    var name = form.ItemName;

                    var project = this.Solution.GetDomainProject();

                    var path = this.SelectedItem.IsFolder() ? this.ProjectItem.GetRelativeFolder() : null;

                    var concept = this.AddFile(form.SelectedTemplate, $"{(path != null ? path + "\\" : "")}{name}.cs", name, project);

                    this.WriteOutput(concept);

                    return concept;
                }
            }
            return null;
        }
    }
}