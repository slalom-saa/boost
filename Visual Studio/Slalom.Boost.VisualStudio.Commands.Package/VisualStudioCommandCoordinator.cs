using System;
using System.ComponentModel.Design;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Commands;
using Slalom.Boost.VisualStudio.RuntimeBinding;
using Task = System.Threading.Tasks.Task;

namespace BoostCommandPack
{
    public sealed class VisualStudioCommandCoordinator
    {
        public static readonly Guid CommandSet = new Guid("e1342b02-f473-4944-b0d1-1396210dea66");

        private readonly Application Application;
        private readonly IContainer container;

        private readonly Microsoft.VisualStudio.Shell.Package package;

        private VisualStudioCommandCoordinator(Microsoft.VisualStudio.Shell.Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException(nameof(package));
            }

            this.package = package;

            container =
                new SimpleContainer(this);

            foreach (var command in container.ResolveAll<IVisualStudioCommand>())
            {
                this.AddCommand(command.Id);
            }

            var dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(DTE)) as DTE;
            Application = new Application(dte);
        }

        public static VisualStudioCommandCoordinator Instance { get; private set; }

        private IServiceProvider ServiceProvider => package;

        private void AddCommand(int commandId)
        {
            var commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;

            if (commandService != null)
            {
                var menuCommandId = new CommandID(CommandSet, commandId);
                var menuItem = new OleMenuCommand(this.MenuItemCallback, menuCommandId);
                menuItem.BeforeQueryStatus += this.HandleMenuItemStatusQuery;
                commandService.AddCommand(menuItem);
            }
        }

        private void HandleMenuItemStatusQuery(object sender, EventArgs e)
        {
            var command = ((OleMenuCommand)sender);
            command.Enabled = command.Visible = false;

            var current = container.ResolveAll<VisualStudioCommand>().FirstOrDefault(x => x.Id == command.CommandID.ID);
            if (current != null && current.ShouldDisplay())
            {
                command.Enabled = command.Visible = true;
            }
        }

        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new VisualStudioCommandCoordinator(package);
        }

        private void MenuItemCallback(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = Application.SelectedItems.First();
                var commandId = ((OleMenuCommand)sender).CommandID.ID;
                var current = container.ResolveAll<VisualStudioCommand>().FirstOrDefault(x => x.Id == commandId);
                current?.HandleCallback(selectedItem.ProjectItem);
            }
            catch { }
        }
    }
}