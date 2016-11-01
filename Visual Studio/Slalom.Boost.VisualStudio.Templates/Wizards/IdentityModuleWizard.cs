using System.Collections.Generic;
using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;
using Slalom.Boost.Templates.SolutionBuilders;

namespace Slalom.Boost.Templates.Wizards
{
    public class IdentityModuleWizard : IWizard
    {
        protected string Data;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            var solution = new IdentityModuleSolutionBuilder(project);
            solution.Create();
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem)
        {
        }

        public void RunFinished()
        {
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Data = replacementsDictionary.ContainsKey("$wizarddata$") ? replacementsDictionary["$wizarddata$"] : "";
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }
}