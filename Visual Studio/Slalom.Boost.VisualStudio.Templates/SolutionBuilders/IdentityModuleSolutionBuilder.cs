using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using EnvDTE;
using EnvDTE80;
using Slalom.Boost.VisualStudio;

namespace Slalom.Boost.Templates.SolutionBuilders
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public class IdentityModuleSolutionBuilder
    {
        private readonly EnvDTE.Project _seedProject;

        public IdentityModuleSolutionBuilder(EnvDTE.Project seedProject)
        {
            _seedProject = seedProject;
            this.Solution = seedProject.DTE.Solution.DTE.Solution;
            this.Solution.SolutionBuild.Build(true);
            this.RootNamespace = (string)seedProject.Properties.Item("DefaultNamespace").Value;
        }

        public Solution Solution { get; }

        public string RootNamespace { get; }

        public string ModuleName => this.RootNamespace.Split('.').Last();

        public EnvDTE.Project DomainProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Domain"));

        public EnvDTE.Project ApplicationProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Application"));

        public EnvDTE.Project PersistenceProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Persistence"));

        public EnvDTE.Project WebApiProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("WebApi"));

        public EnvDTE.Project UnitTestsProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("UnitTests"));

        public EnvDTE.Project IntegrationTestsProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("IntegrationTests"));

        public void Create()
        {
            this.CreateProjects();

            this.Solution.ReplaceTokens(new System.Collections.Generic.Dictionary<string, string> { { "ROOT", this.RootNamespace } });

            this.SetProjectNames();

            this.MoveProjectsToFolders();

            this.SetStartupProperties();

            this.Solution.Delete(_seedProject);

            this.Finish();
        }

        protected virtual void Finish()
        {
            this.WebApiProject.AddReference(this.PersistenceProject);
            this.Solution.CollapseAll();
            this.Solution.SolutionBuild.Build(true);
        }

        protected virtual void SetStartupProperties()
        {
            this.Solution.SetStartupProject(this.WebApiProject.Name);
            this.WebApiProject.SetStartupUrl("swagger");
        }

        protected virtual void SetProjectNames()
        {
            this.Solution.ShortenProjectNames();
        }

        protected virtual void MoveProjectsToFolders()
        {
            this.Solution.MoveProjectToFolder("Infrastructure", this.PersistenceProject, this.WebApiProject);
            this.Solution.MoveProjectToFolder("Testing", this.UnitTestsProject, this.IntegrationTestsProject);
        }

        protected virtual void CreateProjects()
        {
            this.Solution.AddProject("Product.Project.Identity.Domain", this.RootNamespace);
            this.Solution.AddProject("Product.Project.Identity.Application", this.RootNamespace, this.DomainProject);
            this.Solution.AddProject("Product.Project.Identity.Persistence", this.RootNamespace, this.DomainProject, this.ApplicationProject);
            this.Solution.AddProject("Product.Project.Identity.WebApi", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.PersistenceProject);
            this.Solution.AddProject("Product.Project.Module.UnitTests", this.RootNamespace, this.DomainProject, this.ApplicationProject);
            this.Solution.AddProject("Product.Project.Module.IntegrationTests", this.RootNamespace, this.DomainProject, this.ApplicationProject);
        }
    }
}