using System.Data.Entity.Design.PluralizationServices;
using System.Globalization;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers.Files
{
    public class AdminController : ClassBuilder
    {
        public AdminController(ProjectItemDescriptor selectedItem)
            : base(Templates.AdminController, selectedItem)
        {
            this.RelativePath = $"Controllers\\AdminController.cs";
        }
    }
}