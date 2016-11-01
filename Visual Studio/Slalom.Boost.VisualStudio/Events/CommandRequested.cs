using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio.Events
{
    public class CommandRequested : IDomainEvent
    {
        public CommandRequested(string name)
        {
            this.Name = name;
        }

        public CommandRequested(Uri uri) : this(uri.ToString())
        {
            
        }

        public string Name { get; }
    }
}
