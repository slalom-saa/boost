using System.Collections.ObjectModel;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    [IgnoreBinding]
    public interface IAssemblyLocator
    {
        ObservableCollection<_Assembly> Locate(AssemblyFilter[] filters);
    }
}