using System;
using System.Collections.Generic;
using System.Xml.Linq;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio
{
    public static class SelectedItemExtensions
    {
        public static bool IsProject(this SelectedItem item)
        {
            return item != null && item.ProjectItem == null && item.Project != null;
        }

        public static bool IsFolder(this SelectedItem item)
        {
            return item?.ProjectItem != null && item.ProjectItem.Kind == "{6BB5F8EF-4483-11D3-8BCF-00C04F8EC28C}";
        }

        public static bool IsProjectOrFolder(this SelectedItem item)
        {
            return item.IsProject() || item.IsFolder();
        }

        public static bool IsDomainItem(this SelectedItem item)
        {
            return item != null && (item.Project ?? item.ProjectItem.ContainingProject).IsDomainProject();
        }

    }
}