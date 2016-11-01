using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Slalom.Boost.Reflection
{
    public interface IAssemblyLocator
    {
        ObservableCollection<_Assembly> Locate();
    }
}