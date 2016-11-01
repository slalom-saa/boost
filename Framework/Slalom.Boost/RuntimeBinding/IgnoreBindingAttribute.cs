using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Indicates that the class or interface should be ignored when performing runtime binding.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class IgnoreBindingAttribute : Attribute
    {
    }
}