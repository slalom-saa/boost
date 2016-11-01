using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio.Events
{
    public class NavigateRequested : IDomainEvent
    {
        public string Path { get; }

        public NavigateRequested(string path)
        {
            this.Path = path;
        }

        public NavigateRequested(Uri uri) : this(uri.ToString())
        {
        }
    }
}
