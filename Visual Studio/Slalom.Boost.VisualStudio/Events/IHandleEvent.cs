using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Slalom.Boost.VisualStudio.RuntimeBinding;

namespace Slalom.Boost.VisualStudio.Events
{
    [RuntimeBinding(BindingType.Multiple)]
    public interface IHandleEvent<T> where T : IDomainEvent
    {
        void Handle(T instance);
    }
}
