using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using Slalom.Boost.Templates.SolutionBuilders;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;

namespace Slalom.Boost.Templates.Wizards
{
    public class CoreModuleWizard : IWizard
    {
        protected string Data;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            Data = replacementsDictionary.ContainsKey("$wizarddata$") ? replacementsDictionary["$wizarddata$"] : "";
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
            var solution = new CoreModuleSolutionBuilder(project);
            solution.SupressUI();
            solution.Create();
            solution.UnsupressUI();
            solution.Build();
            DomainEvents.Raise(new BoostProjectCreated());
        }
    }
}