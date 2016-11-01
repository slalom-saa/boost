using System.Diagnostics;
using System.Linq;
using System.Text;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddSearchCommandCommand : VisualStudioCommand
    {
        public AddSearchCommandCommand()
            : base(0x0516)
        {
            this.Markers.Add("// TODO: Add search commands [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.ProjectItem.IsReadModel() && !this.Solution.GetApplicationProject().ClassExists($"Search{this.ProjectItem.GetEntityName().Pluralize()}Command");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var entity = item.GetClassName();

            var command = this.AddFile("SearchCommand", $"{entity.Pluralize()}\\Search{entity.Pluralize()}\\Search{entity.Pluralize()}Command.cs", $"Search{entity.Pluralize()}Command", this.Solution.GetApplicationProject(), item);

            var builder = new StringBuilder();
            var properties = item.GetCodeProperties()
                .Where(e => e.Type.AsString == "string")
                .ToList();
            if (properties.Count > 0)
            {
                builder.Append($"\t\t\t\t\t\te => e.{properties[0].Name}.Contains(item)");
                foreach (var source in properties.Skip(1))
                {
                    builder.AppendLine();
                    builder.Append($"\t\t\t\t\t\t|| e.{source.Name}.Contains(item)");
                }
                builder.AppendLine(");");
            }
            else
            {
                builder.AppendLine("\t\t\t\t\t\te => true);");
            }
            var handler = this.AddFile("SearchCommandHandler", $"{entity.Pluralize()}\\Search{entity.Pluralize()}\\Search{entity.Pluralize()}CommandHandler.cs", $"Search{entity.Pluralize()}Command", this.Solution.GetApplicationProject(), item, null,
                new
                {
                    Search = builder.ToString()
                });

            this.WriteOutput(command, handler);

            return handler;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}