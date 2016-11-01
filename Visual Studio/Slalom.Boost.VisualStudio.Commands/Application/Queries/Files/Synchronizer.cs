using System;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files
{
    public class Synchronizer : ClassBuilder
    {
        public Synchronizer(ProjectItemDescriptor projectItem)
            : base(Templates.Synchronizer, projectItem)
        {
            this.Name = projectItem.ClassName;
            this.Entity = projectItem.ClassName.Replace("ReadModel", "");
            this.EntityLower = this.Entity.ToLower();
            this.NameLower = this.Name.ToLowerInvariant();
            this.RelativePath = $"{this.GetSelectedItemRelativePath()}\\{ProjectItem.ClassName}Synchronizer.cs";
        }

        private string GetSelectedItemRelativePath()
        {
            var path = ProjectItem.Path;
            path = path.Substring(path.IndexOf("Application\\", StringComparison.Ordinal) + 12);
            return path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));
        }

        public string Name { get; set; }

        public string EntityLower { get; set; }

        public string Entity { get; set; }

        public string NameLower { get; set; }
    }
}