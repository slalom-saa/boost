using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

namespace Slalom.Boost.VisualStudio
{
    public static class BoostOutputWindow
    {
        private static readonly IVsOutputWindowPane Pane;

        [SuppressMessage("ReSharper", "PossibleNullReferenceException")]
        static BoostOutputWindow()
        {
            var outputWindow = Package.GetGlobalService(typeof(SVsOutputWindow)) as IVsOutputWindow;

            var paneGuid = VSConstants.OutputWindowPaneGuid.GeneralPane_guid;
            outputWindow.CreatePane(paneGuid, "Slalom Boost", 1, 0);
            outputWindow.GetPane(paneGuid, out Pane);
        }

        public static void Write(string message)
        {
            Pane.OutputString(message);
        }

        public static void WriteLine()
        {
            Pane.OutputString(Environment.NewLine);
        }

        public static void WriteLine(string message)
        {
            Pane.OutputString(message + Environment.NewLine);
        }

        public static void Show()
        {
            Pane.Activate();
        }
    }
}