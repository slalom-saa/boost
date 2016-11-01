using System;
using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class QueryCommand : ClassBuilder
    {
        public QueryCommand(ProjectItemDescriptor selectedItem)
            : base(Templates.QueryCommand, selectedItem)
        {
            this.Name = selectedItem.ClassName.Replace("ReadModel", "");
            this.RootNamespace = selectedItem.ClassDescriptor.Namespace;
            this.RelativePath = GetRootPath(selectedItem) + $"\\Query{this.Name.Pluralize()}\\Query{this.Name.Pluralize()}Command.cs";
        }

        public string Name { get; }

        private static string GetRootPath(ProjectItemDescriptor selectedItem)
        {
            var path = selectedItem.Path.Substring(selectedItem.Path.IndexOf("Application\\", StringComparison.Ordinal) + 12)
                .Split('.')[0];
            ;
            path = path.Substring(0, path.LastIndexOf("\\", StringComparison.Ordinal));
            return path;
        }
    }
}