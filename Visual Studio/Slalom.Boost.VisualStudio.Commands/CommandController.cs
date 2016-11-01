using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio.Commands
{
    public class CommandController
    {
        private static IEnumerable<IVisualStudioCommand> commands;

        public IEnumerable<IVisualStudioCommand> GetCommands()
        {
            return commands ?? (commands = Application.Current.Container.ResolveAll<IVisualStudioCommand>());
        }
    }
}
