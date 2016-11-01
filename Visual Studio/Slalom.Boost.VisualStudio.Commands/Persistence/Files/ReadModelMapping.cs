using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files
{
    public class ReadModelMapping : ClassBuilder
    {
        public ReadModelMapping(ProjectItemDescriptor selectedItem)
            : base(Templates.ReadModelMapping, selectedItem)
        {
            this.ReadModel = selectedItem.ClassName;
            this.ReadModelPlural = PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(this.ReadModel);
            this.Namespace = selectedItem.ClassDescriptor.Namespace;
            this.RelativePath = $"Application\\{this.ReadModel}Mapping.cs";
            this.Module = selectedItem.Project.ModuleName;
        }

        public string Namespace { get; set; }

        public string ReadModelPlural { get; set; }

        public string Module { get; set; }

        public string ReadModel { get; set; }
    }
}