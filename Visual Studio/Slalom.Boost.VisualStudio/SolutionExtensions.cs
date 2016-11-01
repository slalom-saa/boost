using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using VSLangProj;

namespace Slalom.Boost.VisualStudio
{
    public static class SolutionExtensions
    {
        private const string SolutionFolderKind = "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}";

        public static EnvDTE.Project AddCoreModule(this Solution instance, string name, params EnvDTE.Project[] references)
        {
            var solution = (Solution2)instance;
            var path = solution.GetProjectTemplate("Core Module", "CSharp");
            solution.AddFromTemplate(path, Path.Combine(Path.GetDirectoryName(solution.FullName), name), name);
            return solution.Projects.Cast<EnvDTE.Project>().FirstOrDefault(e => e.Name == name);
        }

        public static EnvDTE.Project AddProject(this Solution instance, string template, string name, params EnvDTE.Project[] references)
        {
            var solution = (Solution2)instance;
            var path = solution.GetProjectTemplate(template, "CSharp");
            name = name + "." + template.Split('.').Last();
            solution.AddFromTemplate(path, Path.Combine(Path.GetDirectoryName(solution.FullName), name), name);
            var project = solution.Projects.Cast<EnvDTE.Project>().FirstOrDefault(e => e.Name == name);
            if (project != null && references != null)
            {
                foreach (var reference in references)
                {
                    ((VSProject)project.Object).References.AddProject(reference);
                }
            }
            return project;
        }

        public static void CollapseAll(this Solution solution)
        {
            var explorer = (UIHierarchy)solution.DTE.Windows.Item("{3AE79031-E1BC-11D0-8F78-00A0C9110057}").Object;
            if (explorer.UIHierarchyItems.Count == 0)
            {
                return;
            }
            var root = explorer.UIHierarchyItems.Item(1);
            bool suppressed = root.DTE.SuppressUI;
            if (root.DTE.SuppressUI == false)
            {
                root.DTE.SuppressUI = true;
            }

            Collapse(root, explorer);

            root.Select(vsUISelectionType.vsUISelectionTypeSelect);
            root.DTE.SuppressUI = suppressed;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static void SupressUI(this Solution solution)
        {
            var explorer = (UIHierarchy)solution.DTE.Windows.Item("{3AE79031-E1BC-11D0-8F78-00A0C9110057}").Object;
            if (explorer.UIHierarchyItems.Count == 0)
            {
                return;
            }
            var root = explorer.UIHierarchyItems.Item(1);
            root.DTE.SuppressUI = true;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public static void UnsupressUI(this Solution solution)
        {
            var explorer = (UIHierarchy)solution.DTE.Windows.Item("{3AE79031-E1BC-11D0-8F78-00A0C9110057}").Object;
            if (explorer.UIHierarchyItems.Count == 0)
            {
                return;
            }
            var root = explorer.UIHierarchyItems.Item(1);
            root.DTE.SuppressUI = false;
        }

        public static void Delete(this Solution solution, EnvDTE.Project project)
        {
            var path = Path.GetDirectoryName(project.FullName);
            solution.Remove(project);
            Directory.Delete(path, true);
        }

        public static IList<EnvDTE.Project> GetProjects(this Solution instance)
        {
            var list = new List<EnvDTE.Project>();
            var item = instance.Projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as EnvDTE.Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        public static void AddSolutionItem(this Solution instance, string relativePath, byte[] content, bool open = true)
        {
            var path = Path.Combine(instance.GetRootPath(), relativePath);
            var directory = Path.GetDirectoryName(path);
            if (directory != null && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }
            File.WriteAllBytes(path, content);

            if (open)
            {
                var folder = instance.AddSolutionFolder(Path.GetDirectoryName(relativePath));
                folder.ProjectItems.AddFromFile(path);
            }
        }

        [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
        public static Project AddSolutionFolder(this Solution instance, string name)
        {
            var parts = name.Split('\\');

            var current = instance.Projects.Cast<Project>().FirstOrDefault(e => e.Name == parts[0]);
            if (current == null)
            {
                current = ((Solution2)instance).AddSolutionFolder(parts[0]);
            }

            foreach (var part in parts.Skip(1))
            {
                current = current.AddSolutionFolder(part);
            }

            return current;
        }

        public static void MoveProjectToFolder(this Solution instance, string name,
                                               params EnvDTE.Project[] projects)
        {
            var folder = instance.AddSolutionFolder(name);
            foreach (var project in projects)
            {
                var path = project.FullName;
                instance.Remove(project);
                ((SolutionFolder)folder.Object).AddFromFile(path);
            }
        }

        public static void ReplaceTokens(this Solution solution, Dictionary<string, string> replacements)
        {
            foreach (EnvDTE.Project project in solution.Projects)
            {
                ReplaceTokens(project.ProjectItems, replacements);
            }
        }

        public static void SetStartupProject(this Solution solution, string name)
        {
            solution.Properties.Item("StartupProject").Value = name;
        }

        public static void ShortenProjectNames(this Solution solution)
        {
            foreach (EnvDTE.Project item in solution.Projects)
            {
                if (item.Kind != "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}" && item.Name.Count(e => e == '.') > 2)
                {
                    item.Name = string.Join(".", item.Name.Split('.').Skip(2));
                    item.Save();
                }
            }
        }

        private static void Collapse(UIHierarchyItem hierarchyItem, UIHierarchy solutionExplorer)
        {
            foreach (UIHierarchyItem child in hierarchyItem.UIHierarchyItems)
            {
                Collapse(child, solutionExplorer);
                if (child.UIHierarchyItems.Expanded)
                {
                    child.UIHierarchyItems.Expanded = false;
                }
            }
        }

        [SuppressMessage("ReSharper", "EmptyGeneralCatchClause")]
        private static void ReplaceTokens(this ProjectItems projectItems, Dictionary<string, string> replacements)
        {
            foreach (var item in projectItems.Cast<ProjectItem>())
            {
                try
                {
                    var path = item.FileNames[1];
                    if (path.EndsWith(".cs"))
                    {
                        var content = File.ReadAllText(path);
                        var toPath = path;
                        foreach (var key in replacements.Keys)
                        {
                            if (content.Contains(key))
                            {
                                File.WriteAllText(path, content.Replace(key, replacements[key]));
                            }
                            toPath = toPath.Replace(key, replacements[key]);
                        }

                        if (toPath != path)
                        {
                            var inner = item.ContainingProject;
                            item.Remove();
                            File.Move(path, toPath);
                            inner.ProjectItems.AddFromFile(toPath);
                        }
                    }
                }
                catch
                {
                }
                if (item.ProjectItems != null)
                {
                    ReplaceTokens(item.ProjectItems, replacements);
                }
            }
        }

        public static void Build(this Solution solution)
        {
            solution.SolutionBuild.Build();
        }

        public static Project GetApplicationProject(this Solution solution)
        {
            return GetProject(solution, "Application");
        }

        public static Project GetReadModelProject(this Solution solution)
        {
            return GetProject(solution, "ReadModel");
        }

        public static Project GetAutomationProject(this Solution solution)
        {
            return GetProject(solution, "Automation");
        }

        public static Project GetDomainProject(this Solution solution)
        {
            return GetProject(solution, "Domain");
        }

        public static Project GetIntegrationTestProject(this Solution solution)
        {
            return GetProject(solution, "IntegrationTests");
        }

        public static string GetName(this Solution solution)
        {
            return Path.GetFileNameWithoutExtension(solution.FullName);
        }

        public static Project GetPersistenceProject(this Solution solution)
        {
            return GetProject(solution, "Persistence");
        }

        public static Project GetPresentationProject(this Solution solution)
        {
            return GetProject(solution, "Presentation");
        }

        public static Project GetProject(this Solution solution, string name)
        {
            return GetProjects(solution.Projects).FirstOrDefault(e => e.Name.Contains(".") && e.Name == e.Name.Substring(0, e.Name.LastIndexOf('.')) + "." + name);
        }

        public static IList<Project> GetProjects(Projects projects)
        {
            var list = new List<Project>();
            var item = projects.GetEnumerator();
            while (item.MoveNext())
            {
                var project = item.Current as Project;
                if (project == null)
                {
                    continue;
                }

                if (project.Kind == SolutionFolderKind)
                {
                    list.AddRange(GetSolutionFolderProjects(project));
                }
                else
                {
                    list.Add(project);
                }
            }

            return list;
        }

        public static string GetRootPath(this Solution solution)
        {
            return Path.GetDirectoryName(solution.FullName);
        }

        public static IEnumerable<Project> GetSolutionFolderProjects(Project solutionFolder)
        {
            var list = new List<Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                if (subProject.Kind == SolutionFolderKind)
                {
                    list.AddRange(GetSolutionFolderProjects(subProject));
                }
                else
                {
                    list.Add(subProject);
                }
            }
            return list;
        }

        public static Project GetUnitTestProject(this Solution solution)
        {
            return GetProject(solution, "UnitTests");
        }

        public static Project GetWebApiProject(this Solution solution)
        {
            return GetProject(solution, "WebApi");
        }
    }
}