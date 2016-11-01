using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files
{

    public class EntityFrameworkRepositoryBuilder : ClassBuilder
    {
        public EntityFrameworkRepositoryBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.EntityFrameworkRepository, selectedItem)
        {
            this.Entity = selectedItem.ClassName;
            this.PluralEntityName = PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(this.Entity);
            this.PluralEntityNameLower = this.PluralEntityName.ToLower();
            this.RelativePath = $"Domain\\{this.Entity}Repository.cs";
            this.Module = selectedItem.Project.ModuleName;
        }

        public string PluralEntityNameLower { get; set; }

        public string Module { get; set; }

        public string PluralEntityName { get; set; }

        public string Entity { get; set; }
    }
}