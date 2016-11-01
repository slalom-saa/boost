using System;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class DefaultBindingAttribute : Attribute
    {
        public DefaultBindingAttribute()
        {
        }

        public bool Warn { get; set; } = true;
    }
}