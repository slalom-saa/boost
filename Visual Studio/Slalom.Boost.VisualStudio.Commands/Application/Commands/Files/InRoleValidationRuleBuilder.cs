using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class InRoleValidationRuleBuilder : ClassBuilder
    {
        public string Name { get; set; }

        public InRoleValidationRuleBuilder(string name, ProjectItemDescriptor selectedItem)
            : base(Templates.InRoleValidationRule, selectedItem)
        {
            this.Name = name;
            this.Command = this.Name + "Command";
            this.RelativePath = GetRootPath(selectedItem) + $"\\Rules\\user_in_role.cs";
            this.Namespace = GetRootPath(selectedItem).Replace("\\", ".");
        }

        public string Namespace { get; set; }

        public string Command { get; set; }

        private static string GetRootPath(ProjectItemDescriptor selectedItem)
        {
            var path = selectedItem.Path.Substring(selectedItem.Path.IndexOf("Application\\") + 12).Split('.')[0]; ;
            path = path.Substring(0, path.LastIndexOf("\\"));
            return path;
        }
    }
}