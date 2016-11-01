using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EnvDTE;
using Slalom.Boost.Templates;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.RuntimeBinding;

namespace Slalom.Boost.VisualStudio.Commands
{
    public abstract class VisualStudioCommand : IVisualStudioCommand
    {
        private readonly TemplateController _controller = new TemplateController();

        protected VisualStudioCommand(int id)
        {
            this.Id = id;
        }

        public List<string> Markers { get; set; } = new List<string>();

        protected IContainer Container => Application.Current.Container;

        public Solution Solution => Application.Current.Solution;

        public ProjectItem ProjectItem => SelectedItem?.ProjectItem;

        public int Id { get; }

        public SelectedItem SelectedItem => Application.Current.SelectedItems.FirstOrDefault();

        public void HandleCallback(ProjectItem projectItem)
        {
            this.Execute();
        }

        public abstract bool ShouldDisplay();

        public virtual void HandleMarkerClicked()
        {
        }

        protected void WriteOutput(params ProjectItem[] createdItems)
        {
            BoostOutputWindow.WriteLine();
            var projectItems = createdItems.Where(e => e != null).ToList();
            BoostOutputWindow.WriteLine(projectItems.Count + " files created");
            foreach (var item in projectItems)
            {
                BoostOutputWindow.WriteLine($"\t{item.GetPath()}");
            }
            BoostOutputWindow.WriteLine();
            BoostOutputWindow.Show();
        }

        public abstract void Execute();

        protected ProjectItem AddFile(string template, string path, string name, Project project)
        {
            return this.AddFile(template, path, name, project, Enumerable.Empty<CodeProperty>());
        }

        protected ProjectItem AddFile(string template, string path, string name, Project project, IEnumerable<CodeProperty> properties)
        {
            var instance = _controller.GetTemplate(template);

            return project.AddFile(path, instance.Build(name, project, properties));
        }

        protected ProjectItem AddFile(string template, string path, string name, Project project, ProjectItem item, IEnumerable<CodeProperty> properties, object replacements = null)
        {
            var instance = _controller.GetTemplate(template);

            return project.AddFile(path, instance.Build(name, project, item, properties, replacements));
        }


        protected ProjectItem AddFile(string template, string path, string name, Project project, ProjectItem item)
        {
            var instance = _controller.GetTemplate(template);
            return project.AddFile(path, instance.Build(name, project, item));
        }

        protected ProjectItem AddFile(string template, string path, Project project, ProjectItem item)
        {
            var instance = _controller.GetTemplate(template);
            return project.AddFile(path, instance.Build(item.GetClassName(), project, item));
        }

        protected void Open(ProjectItem projectItem)
        {
            if (projectItem != null)
            {
                if (!projectItem.IsOpen)
                {
                    projectItem.Open();
                }
                projectItem.Document?.Activate();
            }
        }

        protected ProjectItem AddFile(string template, string path, Project project, ProjectItem item, object replacements)
        {
            var instance = _controller.GetTemplate(template);
            return project.AddFile(path, instance.Build(item.GetClassName(), project, item, replacements));
        }
    }
}