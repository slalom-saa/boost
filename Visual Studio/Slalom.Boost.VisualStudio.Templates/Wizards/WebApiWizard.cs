using System.Collections.Generic;
using System.IO;
using System.Linq;
using Microsoft.VisualStudio.TemplateWizard;

namespace Slalom.Boost.Templates.Wizards
{
    // TODO: Replace with Wizard Data
    public class WebApiWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }

        public void RunFinished()
        {
        }

        public void BeforeOpeningFile(EnvDTE.ProjectItem projectItem)
        {
        }

        public void ProjectItemFinishedGenerating(EnvDTE.ProjectItem projectItem)
        {
            
        }

        public void ProjectFinishedGenerating(EnvDTE.Project project)
        {
            UpdateContent(project.ProjectItems, project.Name.Split('.')[0]);
        }

        private static void UpdateContent(EnvDTE.ProjectItems items, string name)
        {
            foreach (var item in items.Cast<EnvDTE.ProjectItem>())
            {
                try
                {
                    var path = item.FileNames[1];
                    if (path.EndsWith(".cs"))
                    {
                        var content = File.ReadAllText(path);
                        if (content.Contains("MODULEX"))
                        {
                            File.WriteAllText(path, content.Replace("MODULEX", name));
                        }
                    }
                    if (path.Contains("MODULEX"))
                    {
                        var project = item.ContainingProject;
                        item.Remove();
                        var updated = path.Replace("MODULEX", name);
                        File.Move(path, updated);
                        project.ProjectItems.AddFromFile(updated);
                    }
                }
                catch { }
                UpdateContent(item.ProjectItems, name);
            }
        }
    }
}
