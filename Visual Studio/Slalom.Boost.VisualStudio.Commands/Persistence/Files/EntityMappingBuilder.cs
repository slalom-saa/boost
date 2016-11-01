using System;
using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Persistence.Files
{
    public class EntityMappingBuilder : ClassBuilder
    {
        public EntityMappingBuilder(ProjectItemDescriptor selectedItem)
            : base(Templates.EntityMapping, selectedItem)
        {
            this.Namespace = this.RootNamespace + ".Domain.Model";
            this.RelativePath = "Domain" + $"\\{selectedItem.ClassName}Mapping.cs";
        }

        public string Namespace { get; set; }
    }
}