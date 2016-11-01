using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.IntegrationTests
{
    public class AddIntegrationTests : VisualStudioCommand
    {
        public AddIntegrationTests()
            : base(0x0500)
        {
        }

        public override bool ShouldDisplay()
        {
            return this.ProjectItem.IsCommand();
        }

        public override void Execute()
        {
            if (this.ProjectItem.IsCommand())
            {
                var area = this.ProjectItem.GetRelativeFolder();
                var command = this.ProjectItem.GetClassName();


                var builder = new StringBuilder();
                var properties = this.ProjectItem.GetCodeProperties().ToList();
                if (properties.Any())
                {
                    var prop = properties.First();
                    builder.Append($"{prop.Type.AsString} {prop.Name.ToLower()}");
                    foreach (var source in properties.Skip(1))
                    {
                        builder.Append($", {source.Type.AsString} {source.Name.ToLower()}");
                    }
                }
                var arguments = builder.ToString();

                builder.Clear();
                if (properties.Any())
                {
                    var prop = properties.First();
                    builder.Append($"{prop.Name.ToLower()}");
                    foreach (var source in properties.Skip(1))
                    {
                        builder.Append($", {source.Name.ToLower()}");
                    }
                }
                var argumentNames = builder.ToString();

                builder.Clear();
                if (properties.Any())
                {
                    var prop = properties.First();
                    builder.Append($"default({prop.Type.AsString})");
                    foreach (var source in properties.Skip(1))
                    {
                        builder.Append($", default({source.Type.AsString})");
                    }
                }
                var defaultArguments = builder.ToString();

                var returnType = this.ProjectItem.GetBaseClasses().First().FullName;
                returnType = Regex.Match(returnType, "Command<(.*)>").Groups[1].Value;

                var fixture = this.AddFile("CommandIntegrationTestSetup", $"Application\\{area}\\given\\a_configured_{command.ToSnakeCase()}.cs", this.Solution.GetIntegrationTestProject(), this.ProjectItem, new
                {
                    Module = this.ProjectItem.ContainingProject.GetModuleName(),
                    Area = this.ProjectItem.GetRelativeFolder().Replace("\\", "."),
                    Command = this.ProjectItem.GetClassName().Replace("Command", ""),
                    ReturnType = returnType,
                    Arguments = arguments,
                    ArgumentNames = argumentNames
                });

                var test = this.AddFile("CommandIntegrationTest", $"Application\\{area}\\When_calling_a_{command.ToSnakeCase()}_with_default_values.cs", this.Solution.GetIntegrationTestProject(), this.ProjectItem, new {
                    Module = this.ProjectItem.ContainingProject.GetModuleName(),
                    Area = this.ProjectItem.GetRelativeFolder().Replace("\\", "."),
                    Command = this.ProjectItem.GetClassName().Replace("Command", ""),
                    ReturnType = returnType,
                    Arguments = arguments,
                    ArgumentNames = argumentNames,
                    DefaultArguments = defaultArguments
                });

                this.WriteOutput(fixture, test);

                this.Open(test);
            }
        }
    }
}
