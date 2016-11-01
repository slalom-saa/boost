using System;
using System.Collections.Generic;
using System.Linq;
using Humanizer;
using Slalom.Boost.Extensions;
using Slalom.Boost.Runtime.Humanizer;
using Slalom.Boost.VisualStudio.IDE;

namespace Slalom.Boost.VisualStudio.Projects.Module.Application.Commands.Files
{
    public class ProcessManagerBuilder : ClassBuilder
    {
        public ProcessManagerBuilder(string name, ProjectItemDescriptor selectedItem)
            : base(Templates.ProcessManager, selectedItem)
        {
            this.Name = name;
            this.RelativePath = $"Commands\\{name.Pluralize()}\\{name}ProcessManager.cs";
        }

        public string Name { get; }
    }
}