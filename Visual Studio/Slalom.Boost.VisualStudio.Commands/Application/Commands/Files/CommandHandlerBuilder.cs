using System.IO;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class CommandHandlerBuilder : ClassBuilder
    {
        public string Name { get; set; }

        public CommandHandlerBuilder(string name, ProjectItemDescriptor selectedItem, ProjectItemDescriptor command)
            : base(Templates.CommandHandler, selectedItem)
        {
            this.Name = name;
            this.NameLower = this.Name.ToLower();
            this.RootNamespace = command.ClassDescriptor.Namespace;
            this.RelativePath = command.Path.Substring(command.Project.RootPath.Length + 1).Replace(Path.GetFileName(command.Path), "") + $"{this.Name}CommandHandler.cs";
        }

        public string NameLower { get; set; }
    }
}