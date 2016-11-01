using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Slalom.Boost.VisualStudio.IDE;
using VSLangProj;

namespace Slalom.Boost.VisualStudio
{
    public static class ProjectExtensions
    {
        //internal readonly EnvDTE.Project Project;

        //public Project(EnvDTE.Project project)
        //{
        //    _project = project;
        //}

        //public static Project Current
        //{
        //    get
        //    {
        //        var dte = Package.GetGlobalService(typeof(SDTE)) as DTE2;
        //        var project = dte?.ActiveDocument?.ProjectItem?.ContainingProject;
        //        return project != null ? new Project(project) : null;
        //    }
        //}

        // public string ActiveDocument => _project.DTE.ActiveDocument.FullName;

        public static void SetStartupUrl(this EnvDTE.Project project, string url)
        {
            project.Properties.Item("WebApplication.StartPageUrl").Value = url;
            project.Properties.Item("WebApplication.DebugStartAction").Value = 1;
            project.Save();
        }

        public static void AddReference(this EnvDTE.Project project, params EnvDTE.Project[] references)
        {
            foreach (var reference in references)
            {
                ((VSProject)project.Object).References.AddProject(reference);
            }
        }

        public static bool ContainsFile(this Project project, string name)
        {
            return project.GetProjectItems().Any(e => e.Name == name);
        }

        public static Project AddSolutionFolder(this Project root, string name)
        {
            var item = root.ProjectItems.Cast<ProjectItem>().FirstOrDefault(e => e.Name == name);
            if (item == null)
            {
                return ((SolutionFolder)root.Object).AddSolutionFolder(name);
            }
            return (Project)item.Object;
        }

        public static ProjectItem AddFile(this Project project, string relativePath, string content)
        {
            var path = Path.Combine(project.GetRootPath(), relativePath);
            if (!File.Exists(path))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
                File.WriteAllText(path, content);
                while (!File.Exists(path))
                {
                    System.Threading.Thread.Sleep(250);
                }
                var item = project.ProjectItems.AddFromFile(path);
                project.Save();
                return item;
            }
            return null;
        }

        //// public Solution Solution => new Solution(_project.DTE.Solution);

        public static bool ClassExists(this Project project, string name)
        {
            return project.GetProjectItems().Any(e => e.GetClassName() == name);
        }

        public static bool FileExists(this Project project, string relativePath)
        {
            return File.Exists(Path.Combine(project.GetRootPath(), relativePath));
        }

        // public ProjectItemsExtensions ProjectItemsExtensions => new ProjectItemsExtensions(_project.ProjectItems);

        public static IEnumerable<ProjectItem> GetProjectItems(this Project project)
        {
            return project.ProjectItems.Cast<ProjectItem>().SelectMany(e => e.GetProjectItems());
        }

        public static IEnumerable<ProjectItem> GetProjectItems(this ProjectItem projectItem)
        {
            yield return projectItem;
            foreach (var child in projectItem.ProjectItems.Cast<ProjectItem>())
            {
                yield return child;
                foreach (var sub in GetProjectItems(child))
                {
                    yield return sub;
                }
            }
        }

        // public IEnumerable<ProjectItemDescriptor> GetProjectItems()
        // {
        //     return this.ProjectItemsExtensions.Cast<EnvDTE.ProjectItem>().Select(e => new ProjectItemDescriptor(e));
        // }

        public static bool IsDomainProject(this Project project)
        {
            return project.Name.EndsWith(".Domain");
        }

        // public bool IsApplicationProject => this.Name.EndsWith(".Application");

        // public bool IsPresentationProject => this.Name.EndsWith(".Presentation");

        // public Project UnitTestProject => this.Name.EndsWith("UnitTests") ? null : this.Solution.Projects.FirstOrDefault(e => e.Name == this.Name + ".UnitTests");

        // public Project IntegrationTestProject => this.Name.EndsWith("IntegrationTests") ? null : this.Solution.Projects.FirstOrDefault(e => e.Name == this.Name + ".IntegrationTests");

        // public Project EntityFrameworkProject => this.Name.EndsWith("EntityFramework") ? null : this.Solution.Projects.FirstOrDefault(e => e.Name == this.Name + ".EntityFramework");

        // public string GetFileContent(string relativePath)
        // {
        //     return File.ReadAllText(Path.Combine(this.RootPath, relativePath));
        // }

        // public bool IsEntityFramework => this.Id == new Guid("{69859477-2E3F-47AB-993B-45D853728A75}");

        // public string ModuleName => this.RootNameSpace.Split('.')[this.RootNameSpace.Split('.').Length - 2];

        // public bool IsIntegrationTests => this.Id == new Guid("{9922B4A5-0BC1-4A9C-8BB9-3B9031D77B42}");
        // public bool IsWebApi => this.Id == new Guid("{5B28354B-3C5E-489C-B756-9DF3719D5C67}");

        // public string Name => _project.Name;

        public static void AddProjectReferences(this Project _project, params Project[] projects)
        {
            foreach (var item in projects)
            {
                var project = (VSLangProj.VSProject)_project.Object;
                project.References.AddProject(item);
            }

        }

        public static string GetRootNamespace(this Project project)
        {
            return project.Properties.Item("DefaultNamespace").Value.ToString();
        }

        public static string GetModuleName(this Project project)
        {
            var name = project.GetRootNamespace();
            return name.Substring(0, name.LastIndexOf('.'));
        }

        public static string GetRootPath(this Project project)
        {
            return Path.GetDirectoryName(project.FullName);
        }

        // public Guid Id => new Guid(XDocument.Load(_project.FullName).Descendants().First(e => e.Name.LocalName == "ProjectGuid").Value);
    }
}