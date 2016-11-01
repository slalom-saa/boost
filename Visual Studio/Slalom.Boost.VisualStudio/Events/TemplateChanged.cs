using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio.Events
{
    public class TemplateChanged : IDomainEvent
    {
        public TemplateChanged(string name)
        {
            this.Name = name;
        }

        public string Name { get; set; }
    }

    public interface IDomainEvent
    {
    }
}
