using System;
using System.Diagnostics;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Defines a mapping between an interface and an implementor.
    /// </summary>
    [DebuggerDisplay("{Contract} - {Implementation}")]
    public class ContractMapping
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ContractMapping"/> class.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="implementation">The implementation.</param>
        public ContractMapping(Type contract, Type implementation)
        {
            this.Contract = contract;
            this.Implementation = implementation;
        }

        /// <summary>
        /// Gets the contract.
        /// </summary>
        /// <value>The contract.</value>
        public Type Contract { get; private set; }

        /// <summary>
        /// Gets the implementation.
        /// </summary>
        /// <value>The implementation.</value>
        public Type Implementation { get; private set; }
    }
}