using System;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries
{
    public class AddReadModelCommand : VisualStudioCommand
    {
        public AddReadModelCommand()
            : base(0x0407)
        {
            this.Markers.Add("// TODO: Add a read model for queries [Boost]");
        }

        public override bool ShouldDisplay()
        {
            var entity = this.ProjectItem.GetClassName();

            return this.ProjectItem.IsEntity() && !this.Solution.GetReadModelProject().FileExists($"{entity.Pluralize()}\\{entity}.cs");
        }

        public override void Execute()
        {
            Task.Factory.StartNew(() =>
            {
                this.Open(this.Execute(this.ProjectItem));
            });
        }

        public ProjectItem Execute(ProjectItem projectItem)
        {
            var entity = projectItem.GetClassName();

            if (!this.Solution.GetReadModelProject().FileExists($"{entity.Pluralize()}\\{entity}.cs"))
            {
                var builder = new StringBuilder();
                builder.AppendLine("\t\t/// <summary>");
                builder.AppendLine($"\t\t/// Gets or sets the {entity} identifier.");
                builder.AppendLine("\t\t/// </summary>");
                builder.AppendLine("\t\t/// <value>");
                builder.AppendLine($"\t\t/// The {entity} identifier.");
                builder.AppendLine("\t\t/// </value>");
                builder.Append($"\t\tpublic Guid {entity}Id {{ get; set; }}");
                var read = this.AddFile("ReadModel", $"{entity.Pluralize()}\\{entity}.cs", this.Solution.GetReadModelProject(), projectItem, new
                {
                    IdProperty = builder.ToString()
                });

                builder.Clear();
                builder.AppendLine($"\t\t\ttarget.{entity}Id = instance.{entity}.Id;");
                foreach (var item in projectItem.GetCodeProperties())
                {
                    builder.AppendLine($"\t\t\ttarget.{item.Name} = instance.{entity}.{item.Name};");
                }

                var mapping = this.AddFile("ReadModelMapping", $"ReadModel\\{entity}Mapping.cs", this.Solution.GetPersistenceProject(), read, new
                {
                    Mapping = builder.ToString()
                });
                var sync = this.AddFile("Synchronizer", $"{entity.Pluralize()}\\{entity}Synchronizer.cs", this.Solution.GetReadModelProject(), projectItem, new {
                    Mapping = builder.ToString()
                });

                this.WriteOutput(read, mapping, sync);

                return sync;
            }
            return null;
        }
    }
}