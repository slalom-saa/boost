﻿using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Defines a contract for a business validation rule.
    /// </summary>
    /// <typeparam name="TCommand">The type of command to validate.</typeparam>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IBusinessValidationRule<in TCommand> : IValidationRule<TCommand, CommandContext> where TCommand : ICommand
    {
        /// <summary>
        /// Gets the order of the business rule.  Lower numbers run first.
        /// </summary>
        /// <value>The order of the business rule.</value>
        int Order { get; }
    }
}