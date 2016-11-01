using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.VisualStudio.IDE
{
    [IgnoreBinding]
    public class Solution
    {
        private readonly EnvDTE.Solution _solution;

        public IEnumerable<Project> Projects => GetProjects(_solution.Projects).Select(e => new Project(e));

        public Project AddProject(string name, string template)
        {
            var path = ((Solution2)_solution.DTE.Solution).GetProjectTemplate(template, "CSharp");
            _solution.AddFromTemplate(path, Path.Combine(this.RootPath, name), name);
            return this.Projects.First(e => e.Name == name);
        }

        public string RootPath => Path.GetDirectoryName(_solution.FullName);

        public static Solution Current
        {
            get
            {
                var dte = Package.GetGlobalService(typeof(SDTE)) as DTE2;
                var solution = dte?.Solution;
                return solution != null ? new Solution(solution) : null;
            }
        }


        public string Name => Path.GetFileNameWithoutExtension(_solution.FullName);

        public void Build()
        {
            _solution.SolutionBuild.Build();
        }

        public Solution(EnvDTE.Solution solution)
        {
            _solution = solution;
        }

        public static IList<EnvDTE.Project> GetProjects(EnvDTE.Projects projects)
        {
            List<EnvDTE.Project> list = new List<EnvDTE.Project>();
            var item = projects.GetEnumerator();
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

        private static IEnumerable<EnvDTE.Project> GetSolutionFolderProjects(EnvDTE.Project solutionFolder)
        {
            List<EnvDTE.Project> list = new List<EnvDTE.Project>();
            for (var i = 1; i <= solutionFolder.ProjectItems.Count; i++)
            {
                var subProject = solutionFolder.ProjectItems.Item(i).SubProject;
                if (subProject == null)
                {
                    continue;
                }

                // If this is another solution folder, do a recursive call, otherwise add
                if (subProject.Kind == "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
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
    }

}