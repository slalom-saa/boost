using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using EnvDTE;
using Microsoft.VisualStudio.Shell;
using Slalom.Boost.VisualStudio.RuntimeBinding;

namespace Slalom.Boost.VisualStudio
{
    public class Application
    {
        private readonly DTE _dte;

        public Application(DTE dte)
        {
            _dte = dte;
            this.Container = new SimpleContainer(this);
        }

        public void CreateSolution(string path)
        {
            if (!Directory.Exists(Path.GetDirectoryName(path)))
            {
                Directory.CreateDirectory(Path.GetDirectoryName(path));
            }
            _dte.Solution.Create(Path.GetDirectoryName(path), Path.GetFileName(path));
            _dte.Solution.SaveAs(path);
            _dte.Solution.AddCoreModule(String.Join(".", Path.GetFileNameWithoutExtension(path)));
            //_dte.Solution.Open(path);
        }

        public Document ActiveDocument => _dte?.ActiveDocument;

        public Project ActiveProject => this.ActiveDocument?.ProjectItem?.ContainingProject;

        public IContainer Container { get; }

        public static Application Current => new Application(Package.GetGlobalService(typeof(DTE)) as DTE);

        public IEnumerable<SelectedItem> SelectedItems => _dte.SelectedItems.Cast<SelectedItem>();

        public Solution Solution => _dte?.Solution;

        public static void SuppressUI()
        {
            Current._dte.SuppressUI = true;
        }

        public static void UnsupressUI()
        {
            Current._dte.SuppressUI = false;
        }
    }
}