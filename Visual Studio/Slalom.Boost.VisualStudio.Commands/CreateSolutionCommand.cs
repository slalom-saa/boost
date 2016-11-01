using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Slalom.Boost.VisualStudio.Commands.Domain;
using Slalom.Boost.VisualStudio.Events;
using Slalom.Boost.VisualStudio.Forms;
using Slalom.Boost.VisualStudio.IDE;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Commands;
using Slalom.Boost.VisualStudio.Projects.Module.Application.Queries;
using Slalom.Boost.VisualStudio.Projects.Module.Automation;
using Slalom.Boost.VisualStudio.Projects.Module.WebApi.Controllers;

namespace Slalom.Boost.VisualStudio.Commands
{
    public class CreateSolutionCommand : IHandleEvent<CommandRequested>
    {
        public void Handle(CommandRequested instance)
        {
            if (instance.Name == "CreateSolution")
            {
                var form = new CreateSolutionWindow();
                if (form.ShowDialog() == true)
                {
                    Application.Current.CreateSolution(form.SolutionPath);
                }
            }
        }
    }

    public class RunWithoutDebuggingRequestedHandler : IHandleEvent<CommandRequested>
    {
        public void Handle(CommandRequested instance)
        {
            if (instance.Name == "RunWithoutDebugging")
            {
                Application.Current.Solution.SolutionBuild.Debug();
                Application.Current.Solution.DTE.Debugger.DetachAll();
            }
            if (instance.Name == "OpenDocuments" && !String.IsNullOrWhiteSpace(Application.Current.Solution?.GetRootPath()))
            {
                Process.Start(Path.Combine(Application.Current.Solution.GetRootPath(), "Documents"));
            }
        }
    }

    public class AddAggregateCommandHandler : IHandleEvent<CommandRequested>
    {
        public void Handle(CommandRequested instance)
        {
            if (instance.Name == "AddAggregate")
            {
                var addAggregateCommand = new AddAggregateCommand();
                var aggregate = addAggregateCommand.Execute(Application.Current.Solution);
                if (aggregate != null)
                {
                    var other = new AddAddAggregateCommandCommand();
                    var command = other.Execute(aggregate);

                    var addReadModelCommand = new AddReadModelCommand();
                    var readModel = addReadModelCommand.Execute(aggregate);

                    var queryCommand = new AddSearchCommandCommand();
                    queryCommand.Execute(readModel);

                    var controller = new AddCommandControllerCommand();
                    controller.Execute(aggregate);

                    var script = new AddPowerShellScriptCommand();
                    script.Execute(command);

                    Application.Current.Solution.UnsupressUI();

                    if (!aggregate.IsOpen)
                    {
                        aggregate.Open();
                    }
                    aggregate.Document?.Activate();
                }
            }
        }
    }
}
