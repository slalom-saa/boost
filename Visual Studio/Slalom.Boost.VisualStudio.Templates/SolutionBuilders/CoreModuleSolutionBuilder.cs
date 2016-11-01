using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using EnvDTE80;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.Templates.SolutionBuilders
{
    [SuppressMessage("ReSharper", "SuspiciousTypeConversion.Global")]
    public class CoreModuleSolutionBuilder
    {
        private readonly EnvDTE.Project _seedProject;

        public CoreModuleSolutionBuilder(EnvDTE.Project seedProject)
        {
            _seedProject = seedProject;
            this.Solution = seedProject.DTE.Solution;
            this.RootNamespace = (string)seedProject.Properties.Item("DefaultNamespace").Value;
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public void SupressUI()
        {
            Solution.SupressUI();
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        public void UnsupressUI()
        {
            Solution.UnsupressUI();
        }

        public Solution Solution { get; }

        public string RootNamespace { get; }

        public string ModuleName => this.RootNamespace.Split('.').Last();

        public EnvDTE.Project DomainProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Domain"));

        public EnvDTE.Project ReadModel => this.Solution.GetProjects().First(e => e.Name.EndsWith("ReadModel"));

        public EnvDTE.Project ApplicationProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Application"));

        public EnvDTE.Project AutomationProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Automation"));

        public EnvDTE.Project PersistenceProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("Persistence"));

        public EnvDTE.Project WebApiProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("WebApi"));

        public EnvDTE.Project UnitTestsProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("UnitTests"));

        public EnvDTE.Project IntegrationTestsProject => this.Solution.GetProjects().First(e => e.Name.EndsWith("IntegrationTests"));

        public void Create()
        {
            this.AddDocumentation();

            this.CreateProjects();

            this.SetProjectNames();

            this.MoveProjectsToFolders();

            this.SetStartupProperties();

            this.Solution.Delete(_seedProject);

            this.Finish();
        }

        private void AddDocumentation()
        {
            this.Solution.AddSolutionItem("Documents\\Architecture\\Design Concepts\\Layered Architecture.pptm", Documents.Advanced_Layered_Architecture, false);
        }

        protected virtual void Finish()
        {
            this.WebApiProject.AddReference(this.PersistenceProject);
            this.AutomationProject.AddReference(this.PersistenceProject);

            var moduleName = this.AutomationProject.GetModuleName();
            
            // Update automation project
            var item = this.AutomationProject.GetProjectItems().First(e => e.Name == "Import Data.ps1");
            var content = item.GetContent().Replace("Product.Project.Module.Automation", this.AutomationProject.GetRootNamespace());
            content = content.Replace("Data Source=localhost;Initial Catalog=Product;Integrated Security=true", $"Data Source=localhost;Initial Catalog={moduleName};Integrated Security=true");
            content = content.Replace("Add-ConnectionString -Name 'Product' -Value $ConnectionString", $"Add-ConnectionString -Name '{moduleName}' -Value $ConnectionString");
            File.WriteAllText(item.GetPath(), content);

            // update persistence project
            item = this.PersistenceProject.GetProjectItems().First(e => e.Name == "DataContext.cs");
            content = item.GetContent().Replace("________", moduleName);
            File.WriteAllText(item.GetPath(), content);

            item = this.WebApiProject.GetProjectItems().First(e => e.Name == "Web.config");
            content = item.GetContent().Replace(@"<add name=""Module"" connectionString=""Server=localhost;Database=Module;Integrated Security=true;"" providerName=""System.Data.SqlClient"" />",
                $@"<add name=""{moduleName}"" connectionString=""Server=localhost;Database={moduleName};Integrated Security=true;"" providerName=""System.Data.SqlClient"" />");
            File.WriteAllText(item.GetPath(), content);

            this.Solution.CollapseAll();
        }

        public void Build()
        {
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
            this.Solution.MoveProjectToFolder("Infrastructure", this.PersistenceProject, this.WebApiProject, this.AutomationProject);
            this.Solution.MoveProjectToFolder("Testing", this.UnitTestsProject, this.IntegrationTestsProject);
        }

        protected virtual void CreateProjects()
        {
            this.Solution.AddProject("Product.Project.Module.Domain", this.RootNamespace);
            this.Solution.AddProject("Product.Project.Module.ReadModel", this.RootNamespace, this.DomainProject);
            this.Solution.AddProject("Product.Project.Module.Application", this.RootNamespace, this.DomainProject, this.ReadModel);
            this.Solution.AddProject("Product.Project.Module.Persistence", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.ReadModel);
            this.Solution.AddProject("Product.Project.Module.WebApi", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.PersistenceProject, this.ReadModel);
            this.Solution.AddProject("Product.Project.Module.UnitTests", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.ReadModel);
            this.Solution.AddProject("Product.Project.Module.IntegrationTests", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.ReadModel);
            this.Solution.AddProject("Product.Project.Module.Automation", this.RootNamespace, this.DomainProject, this.ApplicationProject, this.PersistenceProject, this.ReadModel);
        }
    }
}