using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Slalom.Boost.Commands
{
    public interface IHaveSuccessfulFlag
    {
        bool Successful { get; }
    }
}
