using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.UnitTests
{
    public class AddUnitTests : VisualStudioCommand
    {
        public AddUnitTests()
            : base(0x0401)
        {
            this.Markers.Add("// TODO: Add unit tests [Boost]");
            this.Markers.Add("// TODO: Add unit tests for factories and methods [Boost]");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            if (item.IsInputValidation())
            {
                var fixture = this.AddFile("InputValidationUnitTestSetup", "Application\\" + (item.GetRelativeFolder() != "" ? item.GetRelativeFolder() + "\\" : "") + $"\\given\\a_configured_{item.GetClassName()}_rule.cs", item.GetClassName(),
                    this.Solution.GetUnitTestProject(), item);

                var test = this.AddFile("InputValidationUnitTest", "Application\\" + item.GetRelativeFolder() + $"\\When_validating_{this.GetCommand(item).GetClassName().ToSnakeCase()}_command_input_with_invalid_input.cs", item.GetClassName(),
                    this.Solution.GetUnitTestProject(), item);

                this.WriteOutput(fixture, test);

                return test;
            }
            if (item.IsEntity())
            {
                var target = new List<ProjectItem>();
                foreach (var method in item.GetMethods().Where(e => e.Access != vsCMAccess.vsCMAccessPrivate && e.Name != item.GetClassName()))
                {
                    var builder = new StringBuilder();
                    if (method.Parameters.OfType<CodeElement>().Any())
                    {
                        builder.Append("default(" + method.Parameters.OfType<CodeParameter>().First().Type.AsString + ")");
                        foreach (var parameter in method.Parameters.OfType<CodeParameter>().Skip(1))
                        {
                            builder.Append(", ");
                            builder.Append("default(" + parameter.Type.AsString + ")");
                        }
                    }

                    target.Add(this.AddFile(method.IsShared ? "StaticAggregateUnitTest" : "AggregateUnitTest", $"Domain\\{(item.GetRelativeFolder() != "" ? item.GetRelativeFolder() + "\\" : "")}{method.Name}\\When_calling_{method.Name.ToSnakeCase()}_on_a_{item.GetClassName().ToLower()}_with_default_arguments.cs",
                        this.Solution.GetUnitTestProject(), item, new
                        {
                            Method = method.Name,
                            Arguments = builder.ToString()
                        }));

                }


                this.WriteOutput(target.ToArray());

                return target.First();
            }
            if (item.IsStringConcept())
            {
                var test = this.AddFile("StringConceptUnitTest", $"Domain\\When_using_a_{item.GetClassName().ToLower()}_value_object_with_invalid_value.cs", item.GetClassName(), this.Solution.GetUnitTestProject());

                this.WriteOutput(test);

                return test;
            }
            return null;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null &&
                   this.ProjectItem.GetBaseClasses().Any(e => e.Name == "Entity" || e.Name == "InputValidationRuleSet" || e.Name.StartsWith("ConceptAs"));
        }

        private ProjectItem GetCommand(ProjectItem item)
        {
            var name = Regex.Match(item.GetContent(), @"class\W*\w*\W*:\W*InputValidationRuleSet<(\w*)>", RegexOptions.Multiline).Groups[1].Value;
            return item.ContainingProject.GetProjectItems().First(e => e.GetClassName() == name);
        }

        //{
        //if (this.ProjectItem.IsInputValidation)
        //var project = this.EnsureProject();
        // {

        //protected override void HandleCallback()
        //    new InputValidationUnitTestSetupBuilder(this.ProjectItem).Build(project);
        //    new InputValidationUnitTestBuilder(this.ProjectItem).Build(project);
        //}
        //else if (this.ProjectItem.IsCommandHandler)
        //{
        //    new CommandHandlerUnitTestSetupBuilder(this.ProjectItem).Build(project);
        //    new CommandHandlerUnitTestBuilder(this.ProjectItem).Build(project);
        //}
        //else if (this.ProjectItem.IsBusinessValidation)
        //{
        //    new BusinessValidationUnitTestSetupBuilder(this.ProjectItem).Build(project);
        //    new BusinessValidationUnitTestBuilder(this.ProjectItem).Build(project);
        //}
        // }
    }
}