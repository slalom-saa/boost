using System;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates
{
    public class AddAggregateCommandHandlerTemplate : Template
    {
        private static readonly string TemplateContent = Files.AddAggregateCommandHandlerTemplate;

        public AddAggregateCommandHandlerTemplate()
            : base("AddAggregateCommandHandler", TemplateContent)
        {
        }

        public override string Build(string name, Project project, ProjectItem item, object replacements = null)
        {
            return base.Build(name, project, item).ReplaceTokens(new
            {
                CreationArguments = String.Join(", ", item.GetCodeProperties().Where(e => e.Name != "Id").Select(e => "command." + e.Name))
            });
        }
    }
}