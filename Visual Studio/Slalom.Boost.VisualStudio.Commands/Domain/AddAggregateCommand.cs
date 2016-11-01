using System;
using System.Collections.Generic;
using System.Linq;
using EnvDTE;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands;
using Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers;
using Slalom.Boost.VisualStudio.Projects.Module.Automation;

namespace Slalom.Boost.VisualStudio.Commands.Domain
{
    public class AddAggregateCommand : VisualStudioCommand
    {
        public AddAggregateCommand()
            : base(0x0404)
        {
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem.IsProject() && this.SelectedItem.IsDomainItem();
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.Solution));
        }

        public ProjectItem Execute(Solution solution)
        {
            var form = new AddClassWindow("Add Aggregate", "Member");
            {
                if (form.ShowDialog() == true)
                {
                    if (form.SelectedTemplate != null)
                    {
                        if (form.SelectedTemplate == "Member")
                        {
                            var name = form.ItemName;

                            var project = this.Solution.GetDomainProject();

                            var root = this.AddFile("Member", $"{name.Pluralize()}\\{name}.cs", name, project);
                            //var @event = this.AddFile("AggregateAddedEvent", $"{name.Pluralize()}\\{name}Added.cs", project, root);
                            var concept = this.AddFile("UserName", $"UserName.cs", "UserName", project, Enumerable.Empty<CodeProperty>());

                            //project = this.Solution.GetPersistenceProject();
                            //var mapping = this.AddFile("EntityMapping", $"Entities\\{name}Mapping.cs", project, root);
                            var repository = this.AddFile("MongoRepository", $"Domain\\{name}Repository.cs", this.Solution.GetPersistenceProject(), root);

                            //var read = this.AddFile("MemberReadModel", $"{name.Pluralize()}\\{name}ReadModel.cs", this.Solution.GetApplicationProject(), root);
                            //var mapping2 = this.AddFile("ReadModelMapping", $"ReadModels\\{name}ReadModelMapping.cs", this.Solution.GetPersistenceProject(), read);
                            //var sync = this.AddFile("Synchronizer", $"{name.Pluralize()}\\{name}ReadModelSynchronizer.cs", this.Solution.GetApplicationProject(), root);

                            //var command = new AddAddAggregateCommandCommand().Execute(root);
                            //var script = new AddPowerShellScriptCommand().Execute(command);

                            //var command2 = this.AddFile("QueryCommandWithParameter", $"{name.Pluralize()}\\Query{name.Pluralize()}\\Query{name.Pluralize()}Command.cs", $"Query{name.Pluralize()}Command", this.Solution.GetApplicationProject(), read);
                            //var handler = this.AddFile("QueryMembersCommandHandler", $"{name.Pluralize()}\\Query{name.Pluralize()}\\Query{name.Pluralize()}CommandHandler.cs", this.Solution.GetApplicationProject(), read);

                            //var controller = this.AddFile("CommandControllerWithParameter", $"Controllers\\{name}Controller.cs", this.Solution.GetWebApiProject(), root);

                            this.WriteOutput(root, concept, repository);

                            return root;
                        }
                    }
                    else
                    {
                        var name = form.ItemName;
                        var properties = form.Properties.ToList();

                        var project = this.Solution.GetDomainProject();

                        var root = this.AddFile("Aggregate", $"{name.Pluralize()}\\{name}.cs", name, project, properties);

                        this.WriteOutput(root);

                        return root;
                    }
                }
            }
            return null;
        }
    }
}