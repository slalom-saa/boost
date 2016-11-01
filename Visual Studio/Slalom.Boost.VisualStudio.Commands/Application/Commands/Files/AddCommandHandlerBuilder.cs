using System;
using System.IO;
using System.Linq;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class AddCommandHandlerBuilder : ClassBuilder
    {
        public string Name { get; set; }

        public AddCommandHandlerBuilder(string name, ProjectItemDescriptor selectedItem, ProjectItemDescriptor command)
            : base(Templates.AddCommandHandler, selectedItem)
        {
            this.Name = name;
            this.Entity = selectedItem.ClassName;
            this.NameLower = this.Name.ToLower();
            this.RootNamespace = command.ClassDescriptor.Namespace;
            this.RelativePath = command.Path.Substring(command.Project.RootPath.Length + 1).Replace(Path.GetFileName(command.Path), "") + $"{this.Name}CommandHandler.cs";
            this.CreationArguments = String.Join(", ", selectedItem.ClassDescriptor.Properties.Where(e => e.Name != "Id").Select(e => "command." + e.Name));
        }

        public string CreationArguments { get; set; }

        public string Entity { get; set; }

        public string NameLower { get; set; }
    }
}