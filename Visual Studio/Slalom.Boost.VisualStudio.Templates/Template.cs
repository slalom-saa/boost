using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using EnvDTE;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.RuntimeBinding;

namespace Slalom.Boost.Templates
{
    [RuntimeBinding(BindingType.Multiple)]
    public abstract class Template
    {
        protected Template(string name, string content)
        {
            this.Name = name;
            this.Content = content;
        }

        public string Content { get; set; }

        public string Name { get; set; }

        public virtual string Build(string name, Project project, IEnumerable<CodeProperty> properties, object replacements = null)
        {
            var target = this.Content.ReplaceTokens(new
            {
                Project = project,
                Properties = properties,
                Name = name
            }, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));

            if (replacements != null)
            {
                target = target.ReplaceTokens(replacements, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));
            }

            return target;
        }

        public virtual string Build(string name, Project project, ProjectItem item, object replacements = null)
        {
            var target = this.Content.ReplaceTokens(new
            {
                Project = project,
                Properties = item.GetCodeProperties(),
                ProjectItem = item,
                Name = name
            }, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));

            if (replacements != null)
            {
                target = target.ReplaceTokens(replacements, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));
            }

            return target;
        }

        public virtual string Build(string name, Project project, ProjectItem item, IEnumerable<CodeProperty> properties, object replacements = null)
        {
            var target = this.Content.ReplaceTokens(new
            {
                Project = project,
                Properties = properties,
                ProjectItem = item,
                Name = name
            }, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));

            if (replacements != null)
            {
                target = target.ReplaceTokens(replacements, typeof(CodePropertyExtensions), typeof(ProjectExtensions), typeof(ProjectItemExtensions));
            }

            return target;
        }
    }
}