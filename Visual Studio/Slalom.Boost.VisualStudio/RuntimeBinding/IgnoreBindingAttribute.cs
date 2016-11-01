using System;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class IgnoreBindingAttribute : Attribute
    {
    }
}