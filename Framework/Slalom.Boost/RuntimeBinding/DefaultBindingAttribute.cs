using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Indicates that the class is a default implementation for runtime binding.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [Serializable, AttributeUsage(AttributeTargets.Class | AttributeTargets.Interface)]
    public sealed class DefaultBindingAttribute : Attribute
    {
        /// <summary>
        /// Gets or sets a value indicating whether a warning should be written to the logger when this 
        /// implementation is bound.
        /// </summary>
        /// <value><c>true</c> if warn; otherwise, <c>false</c>.</value>
        public bool Warn { get; set; }
    }
}