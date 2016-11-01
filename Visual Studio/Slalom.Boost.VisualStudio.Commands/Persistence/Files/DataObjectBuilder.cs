using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files
{
    public class DataObjectBuilder : ClassBuilder
    {
        public DataObjectBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.DataObject, selectedItem)
        {
            this.Entity = selectedItem.ClassName;
            this.PluralEntityName = PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(this.Entity);
            this.RelativePath = $"Model\\{this.Entity}.cs";
            this.Module = selectedItem.Project.ModuleName;
        }

        public string Module { get; set; }

        public string PluralEntityName { get; set; }

        public string Entity { get; set; }
    }
}