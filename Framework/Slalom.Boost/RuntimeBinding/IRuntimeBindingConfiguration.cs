using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Defines a class that should be run after the container is initially configured to add additional
    /// configuration.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IRuntimeBindingConfiguration
    {
        /// <summary>
        /// Adds additional configuration to the specified container.
        /// </summary>
        /// <param name="container">The current container instance.</param>
        void Configure(IContainer container);
    }
}