using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class BusinessValidationRuleBuilder : ClassBuilder
    {
        public BusinessValidationRuleBuilder(string name, int index, ProjectItemDescriptor selectedItem)
            : base(Templates.BusinessValidationRule, selectedItem)
        {
            this.Name = name;
            this.Index = index;
            this.Rule = "business_rule" + this.Index;
            this.RelativePath = GetRootPath(selectedItem) + $"\\Rules\\{this.Rule}.cs";
            this.Namespace = GetRootPath(selectedItem).Replace("\\", ".");
        }

        public string Namespace { get; set; }

        private static string GetRootPath(ProjectItemDescriptor selectedItem)
        {
            var path = selectedItem.Path.Substring(selectedItem.Path.IndexOf("Application\\") + 12).Split('.')[0]; ;
            path = path.Substring(0, path.LastIndexOf("\\"));
            return path;
        }

        public string Name { get; set; }

        public int Index { get; set; }

        public string Rule { get; }
    }
}