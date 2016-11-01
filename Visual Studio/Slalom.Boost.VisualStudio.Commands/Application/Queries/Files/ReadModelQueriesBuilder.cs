using System.Collections.Generic;
using System.Linq;
using System.Text;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files
{
    public class ReadModelQueriesBuilder : ClassBuilder
    {
        public ReadModelQueriesBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.ReadModelQueries, selectedItem)
        {
            this.Name = selectedItem.ClassName;
            this.RelativeNamespace = selectedItem.ClassName.Pluralize();
            this.Properties = selectedItem.ClassDescriptor.Properties.ToList();
            this.RelativePath = $"{selectedItem.ClassName.Pluralize()}\\{selectedItem.ClassName}Queries.cs";
        }

        public string RelativeNamespace { get; set; }

        public List<Property> Properties { get; }

        public string Name { get; }

        public string Queries
        {
            get
            {
                var builder = new StringBuilder();
                foreach (var item in this.Properties)
                {
                    builder.AppendLine(
                        $"\t\tpublic static IQueryable<{this.Name}ReadModel> By{item.Name}(this IQueryable<{this.Name}ReadModel> instance, {item.PropertyType} value)");
                    builder.AppendLine("\t\t{");
                    builder.AppendLine($"\t\t\treturn instance.Where(e => e.{item.Name} == value);");
                    builder.AppendLine("\t\t}");
                    if (item != this.Properties.Last())
                    {
                        builder.AppendLine();
                    }
                }
                return builder.ToString();
            }
        }
    }
}