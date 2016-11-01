using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers.Files
{
    public class CommandControllerBuilder : ClassBuilder
    {
        public CommandControllerBuilder(string name, ProjectItemDescriptor selectedItem)
            : base(Templates.CommandController, selectedItem)
        {
            this.Name = name;
            this.RelativePath = $"Controllers\\{this.Name}Controller.cs";
        }

        public string Name { get; }

        public string NameLower => PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(this.Name).ToLower();
    }
}