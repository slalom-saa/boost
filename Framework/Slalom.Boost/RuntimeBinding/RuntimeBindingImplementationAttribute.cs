using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Used to add specifics about the runtime binding implementation.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Class)]
    public sealed class RuntimeBindingImplementationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingContractAttribute"/> class.
        /// </summary>
        /// <param name="type">The binding type for the class or interface.</param>
        public RuntimeBindingImplementationAttribute(ImplementationBindingType type)
        {
            this.BindingType = type;
        }

        /// <summary>
        /// Gets the type of binding for the class or interface.
        /// </summary>
        /// <value>The type of the binding for the class or interface.</value>
        public ImplementationBindingType BindingType { get; }
    }
}