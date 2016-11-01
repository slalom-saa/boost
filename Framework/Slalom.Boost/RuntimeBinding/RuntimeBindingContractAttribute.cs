using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Used to add specifics about the runtime binding contract.
    /// </summary>
    [Serializable, AttributeUsage(AttributeTargets.Interface)]
    public sealed class RuntimeBindingContractAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingContractAttribute"/> class.
        /// </summary>
        /// <param name="type">The binding type for the class or interface.</param>
        public RuntimeBindingContractAttribute(ContractBindingType type)
        {
            this.ContractBindingType = type;
        }

        /// <summary>
        /// Gets the type of binding for the class or interface.
        /// </summary>
        /// <value>The type of the binding for the class or interface.</value>
        public ContractBindingType ContractBindingType { get; }
    }
}