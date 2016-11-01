//------------------------------------------------------------------------------
// <copyright file="BoostLearnWindowCommand.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.ComponentModel.Design;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;

namespace Slalom.Boost.Learn.Package.ToolWindows
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class BoostLearnWindowCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 0x0100;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("331f4a82-bf6c-413c-be4c-36a0a0a16e43");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly Microsoft.VisualStudio.Shell.Package package;

        private IVsWindowFrame _windowFrame;

        /// <summary>
        /// Initializes a new instance of the <see cref="BoostLearnWindowCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        private BoostLearnWindowCommand(Microsoft.VisualStudio.Shell.Package package)
        {
            if (package == null)
            {
                throw new ArgumentNullException("package");
            }

            this.package = package;

            OleMenuCommandService commandService = this.ServiceProvider.GetService(typeof(IMenuCommandService)) as OleMenuCommandService;
            if (commandService != null)
            {
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.ShowToolWindow, menuCommandID);
                commandService.AddCommand(menuItem);
            }

            DomainEvents.Register<BoostProjectCreated>(e =>
            {
                this.ShowToolWindow(this, EventArgs.Empty);
            });
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static BoostLearnWindowCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private IServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static void Initialize(Microsoft.VisualStudio.Shell.Package package)
        {
            Instance = new BoostLearnWindowCommand(package);
        }

        /// <summary>
        /// Shows the tool window when the menu item is clicked.
        /// </summary>
        /// <param name="sender">The event sender.</param>
        /// <param name="e">The event args.</param>
        internal void ShowToolWindow(object sender, EventArgs e)
        {
            // Get the instance number 0 of this tool window. This window is single instance so this instance
            // is actually the only one.
            // The last flag is set to true so that if the tool window does not exists it will be created.
            ToolWindowPane window = this.package.FindToolWindow(typeof(BoostLearnWindow), 0, true);
            if ((null == window) || (null == window.Frame))
            {
                throw new NotSupportedException("Cannot create tool window");
            }

            _windowFrame = (IVsWindowFrame)window.Frame;
            Microsoft.VisualStudio.ErrorHandler.ThrowOnFailure(_windowFrame.Show());
        }

        internal void EnsureWindow()
        {
            if (_windowFrame == null)
            {
                ShowToolWindow(this, EventArgs.Empty);
            }
            else if (_windowFrame.IsVisible() == 1)
            {
                _windowFrame.Show();
            }
        }
    }
}
