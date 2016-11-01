using System;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    [RuntimeBinding(BindingType.Multiple)]
    public interface IRuntimeBindingConfiguration
    {
        void Configure(IContainer container);
    }
}
