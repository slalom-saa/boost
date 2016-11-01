using System;
using System.Linq;
using System.Text;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class InputValidationRuleSetTemplate : Template
    {
        private static readonly string TemplateContent = Files.InputValidationRuleSetTemplate;

        public InputValidationRuleSetTemplate()
            : base("InputValidationRuleSet", TemplateContent)
        {
        }

        public override string Build(string name, Project project, ProjectItem item, object replacements = null)
        {
            return base.Build(name, project, item).ReplaceTokens(new
            {
                Rule = item.GetClassName().ToSnakeCase() + "_is_valid",
                Rules = this.BuildValidationContent(item)
            });
        }

        public string BuildValidationContent(ProjectItem item)
        {
            var properties = item.GetCodeProperties().ToList();
            var builder = new StringBuilder();
            foreach (var property in properties)
            {
                if (property.Type.AsFullName == typeof(String).FullName)
                {
                    builder.AppendLine();
                    builder.AppendLine($"\t\t\tthis.Add(e => e.{property.Name})");
                    builder.Append($"\t\t\t\t.NotNullOrWhiteSpace(\"{property.Name.Humanize(LetterCasing.Sentence)} cannot be null or whitespace.\");");
                }
            }

            return builder.ToString();
        }
    }
}