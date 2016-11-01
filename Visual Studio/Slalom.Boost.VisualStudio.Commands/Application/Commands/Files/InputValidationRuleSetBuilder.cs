using System.Linq;
using System.Text;
using Humanizer;
using Slalom.Boost.Extensions;
using Slalom.Boost.Runtime.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class InputValidationRuleSetBuilder : ClassBuilder
    {
        public string Name { get; set; }

        public string Rule { get; set; }

        public InputValidationRuleSetBuilder(string name, ProjectItemDescriptor selectedItem)
            : base(Templates.InputValidationRuleSet, selectedItem)
        {
            this.Name = name;
            this.Rule = this.Name.ToSnakeCase().ToLower() + "_command_is_valid";
            this.RelativePath = GetRootPath(selectedItem) + $"\\Rules\\{this.Rule}.cs";
            this.Namespace = GetRootPath(selectedItem).Replace("\\", ".");
            this.Rules = this.BuildValidationContent();
        }

        public string Namespace { get; set; }

        private static string GetRootPath(ProjectItemDescriptor selectedItem)
        {
            var path = selectedItem.Path.Substring(selectedItem.Path.IndexOf("Application\\") + 12).Split('.')[0];;
            path = path.Substring(0, path.LastIndexOf("\\"));
            return path;
        }

        public string Rules { get; set; }

        public string BuildValidationContent()
        {
            var properties = this.ProjectItem.ClassDescriptor.Properties.ToList();
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                if (property.PropertyType == "string")
                {
                    builder.AppendLine($"\t\t\tthis.Add(e => e.{property.Name})");
                    builder.AppendLine($"\t\t\t\t.NotNullOrWhiteSpace(\"{property.Name.Humanize(LetterCasing.Sentence)} cannot be null or whitespace.\");");
                    if (properties.Last() != property)
                    {
                        builder.AppendLine();
                    }
                }
            }

            return builder.ToString();
        }
    }
}