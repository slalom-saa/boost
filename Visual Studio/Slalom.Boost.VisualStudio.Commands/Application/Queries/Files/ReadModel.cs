using Slalom.Boost.Extensions;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Queries.Files
{
    public class ReadModel : ClassBuilder
    {

        public ReadModel(ProjectItemDescriptor selectedItem)
            : base(Templates.ReadModel, selectedItem)
        {
            this.Name = selectedItem.ClassName + "ReadModel";
            this.RelativeNamespace = selectedItem.ClassName.Pluralize();
            this.RelativePath = $"{selectedItem.ClassName.Pluralize()}\\{this.Name}.cs";
        }

        public string RelativeNamespace { get; set; }

        public string Name { get; }
    }
}