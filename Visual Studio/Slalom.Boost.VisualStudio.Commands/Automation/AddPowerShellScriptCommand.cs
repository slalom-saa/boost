using EnvDTE;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Automation
{
    public class AddPowerShellScriptCommand : VisualStudioCommand
    {
        public AddPowerShellScriptCommand()
            : base(0x0518)
        {
            this.Markers.Add("// TODO: Add a PowerShell script [Boost]");
        }

        public override bool ShouldDisplay()
        {
            return this.SelectedItem != null && this.ProjectItem.IsAddCommand();
        }

        public ProjectItem Execute(ProjectItem item)
        {
            var rule = this.AddFile("PowerShellCommand", $"Scripts\\Commands\\{item.Name}.ps1", this.Solution.GetAutomationProject(), item);

            this.WriteOutput(rule);

            return rule;
        }

        public override void Execute()
        {
            this.Open(this.Execute(this.ProjectItem));
        }
    }
}