using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Defines a contract for validating commands.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface ICommandValidator
    {
        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The type of response expected for the command.</typeparam>
        /// <param name="command">The command to validate.</param>
        /// <param name="context">The current command context.</param>
        /// <returns>The <see cref="ValidationMessage">messages</see> returned from validation routines.</returns>
        IEnumerable<ValidationMessage> Validate<TResponse>(Command<TResponse> command, CommandContext context);
    }
}