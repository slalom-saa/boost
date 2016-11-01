using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers
{
    public class AddCommandControllerCommand : VisualStudioCommand
    {
        public AddCommandControllerCommand()
            : base(0x0501)
        {
            this.Markers.Add("// TODO: Add an endpoint [Boost]");
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var plural = Path.GetDirectoryName(Path.GetDirectoryName(item.GetPath())).Split('\\').Last();

            var single = plural.Singularize();

            ProjectItem controller;

            if (!this.Solution.GetWebApiProject().ClassExists($"{single}Controller"))
            {
                controller = this.AddFile("CommandController", $"Controllers\\{single}Controller.cs", single, this.Solution.GetWebApiProject(), item, new CodeProperty[0], new
                {
                    Single = single,
                    Plural = plural
                });
            }
            else
            {
                controller = this.Solution.GetWebApiProject().GetProjectItems().First(e => e.GetClassName() == $"{single}Controller");
            }

            var content = File.ReadAllLines(controller.GetPath()).ToList();
            content = content.Take(content.Count() - 2).ToList();

            var replace = item.GetClassName().Replace("Command", "");
            replace = Regex.Replace(replace, @"([a-z])([A-Z])", "$1-$2").ToLower();
            if (replace.Split('-').Count() == 2 && (replace.Split('-')[1] == single.ToLower() || replace.Split('-')[1] == plural.ToLower()))
            {
                replace = replace.Split('-')[0];
            }
            content.Add("");
            if (item.GetBaseClasses().Any(e => e.FullName.Contains("IQueryable")))
            {
                content.Add("\t\t" + $@"[Route(""actions/{replace}""), HttpGet, EnableQuery]");
                content.Add($"\t\tpublic Task<dynamic> Execute(string search = null, bool suggest = false)");
                content.Add("\t\t{");
                content.Add($"\t\t\treturn base.SendAsync(new {item.GetFullClassName()}(search, suggest));");
                content.Add("\t\t}");
                content.Add("\t}");
                content.Add("}");
            }
            else
            {
                content.Add("\t\t" + $@"[Route(""actions/{replace}""), HttpPost]");
                content.Add($"\t\tpublic Task<dynamic> Execute({item.GetFullClassName()} command)");
                content.Add("\t\t{");
                content.Add($"\t\t\treturn base.SendAsync(command);");
                content.Add("\t\t}");
                content.Add("\t}");
                content.Add("}");
            }


            File.WriteAllLines(controller.GetPath(), content);

            this.WriteOutput(controller);

            return controller;
        }

        public override bool ShouldDisplay()
        {
            return this.ProjectItem.IsCommand();
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}