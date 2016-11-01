using System;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    /// <summary>
    /// Indicates a class that is used with runtime binding.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Interface | AttributeTargets.Class)]
    public sealed class RuntimeBindingAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingAttribute"/> class.
        /// </summary>
        /// <param name="type">The binding type for the class or interface.</param>
        public RuntimeBindingAttribute(BindingType type)
        {
            this.BindingType = type;
        }

        /// <summary>
        /// Gets the type of binding for the class or interface.
        /// </summary>
        /// <value>The type of the binding for the class or interface.</value>
        public BindingType BindingType { get; }
    }
}