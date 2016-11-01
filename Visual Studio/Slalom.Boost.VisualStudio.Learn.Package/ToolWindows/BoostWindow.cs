//------------------------------------------------------------------------------
// <copyright file="BoostLearnWindow.cs" company="Company">
//     Copyright (c) Company.  All rights reserved.
// </copyright>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Windows.Input;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Slalom.Boost.Learn.Content;
using Slalom.Boost.VisualStudio;
using Slalom.Boost.VisualStudio.Events;

namespace Slalom.Boost.Learn.Package.ToolWindows
{
    /// <summary>
    /// This class implrements the tool window exposed by this package and hosts a user control.
    /// </summary>
    /// <remarks>
    /// In Visual Studio tool windows are composed of a frame (implemented by the shell) and a pane,
    /// usually implemented by the package implementer.
    /// <para>
    /// This class derives from the ToolWindowPane class provided from the MPF in order to use its
    /// implementation of the IVsUIElementPane interface.
    /// </para>
    /// </remarks>
    [Guid("0bf75040-6a65-412e-a0d5-51c198cbb96a")]
    public class BoostLearnWindow : ToolWindowPane
    {
        private CommandEvents _commandEvents;
        private Document _previous;

        public static BoostLearnWindow Instance { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BoostLearnWindow"/> class.
        /// </summary>
        public BoostLearnWindow() : base(null)
        {
            this.Caption = "Slalom Boost";

            // This is the user control hosted by the tool window; Note that, even if this class implements IDisposable,
            // we are not calling Dispose on this object. This is because ToolWindowPane calls Dispose on
            // the object returned by the Content property.
            this.Content = new BoostLearnWindowControl();

            Instance = this;

            var dte = Microsoft.VisualStudio.Shell.Package.GetGlobalService(typeof(SDTE)) as DTE2;
            _commandEvents = dte.Events.CommandEvents;
            _commandEvents.AfterExecute += (a, b, c, d) =>
            {
                try
                {
                    // TODO: Re-enable to update reference on navigation
                    //var document = Application.Current?.ActiveDocument;
                    //if (document != null && _previous != document)
                    //{
                    //    _previous = document;
                    //    var content = File.ReadAllText(document.Path);
                    //    var controller = new ContentController();
                    //    var key = controller.GetFileKey(content);
                    //    BoostLearnWindowCommand.Instance?.EnsureWindow();
                    //    (Instance?.Content as ReferenceControl)?.OnFileNavigated(key);
                    //}
                }
                catch
                {
                }
            };

            DomainEvents.Register<NavigateRequested>(e =>
            {
                if (e.Path == "Reference")
                {
                    BoostLearnWindowCommand.Instance?.EnsureWindow();
                    (Instance?.Content as BoostLearnWindowControl)?.HandleReferenceClicked(this, null);
                }
            });
        }

        public static void OnTokenClicked(string name)
        {
            try
            {
                BoostLearnWindowCommand.Instance?.EnsureWindow();
                (Instance?.Content as BoostLearnWindowControl)?.OnTokenClicked(name);
            }
            catch
            {
            }
        }
    }
}
