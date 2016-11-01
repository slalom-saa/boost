using System;
using System.Linq;
using System.Windows.Forms;
using Microsoft.Web.Administration;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi
{
    public class BindToIIS : VisualStudioCommand
    {
        public BindToIIS()
            : base(0x0514)
        {
            this.IsAsync = false;
        }

        public override bool ShouldDisplay(ProjectItemDescriptor projectItem)
        {
            return projectItem.Project.IsWebApi && projectItem.IsProject;
        }

        protected override void HandleCallback()
        {
            using (var form = new AddItemForm("Site Name", false))
            {
                if (form.ShowDialog() == DialogResult.OK)
                {
                    var iisManager = new ServerManager();
                    var site = iisManager.Sites.FirstOrDefault(e => e.Bindings.Any(x => x.EndPoint.Port == 80 && string.IsNullOrEmpty(x.Host)));
                    var app = site.Applications.Add("/" + form.ItemName.ToLower(), this.ProjectItem.Project.RootPath);
                    app.ApplicationPoolName = iisManager.ApplicationPools.FirstOrDefault(e => e.Name.StartsWith("Default")).Name;
                    iisManager.CommitChanges();
                }
            }
        }
    }
}