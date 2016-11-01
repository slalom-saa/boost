using System.Threading;
using System.Threading.Tasks;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Coordinates the handling of a command, passing it through the command stack.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface ICommandCoordinator
    {
        /// <summary>
        /// Handles the validation and execution of the specified command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="context"></param>
        /// <returns>A command result with validation errors and data.</returns>
        Task<CommandResult<TResponse>> Handle<TResponse>(Command<TResponse> command, CommandContext context);
    }
}