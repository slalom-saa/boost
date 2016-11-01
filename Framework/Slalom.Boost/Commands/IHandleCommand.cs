using System;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Defines a contract for handling <see cref="Command{TCommand}"/> instances.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <seealso cref="Slalom.Boost.Commands.IHaveCommandContext" />
    public interface IHandleCommand<in TCommand, out TResponse> : IHaveDataFacade, IHaveCommandContext where TCommand : Command<TResponse>
    {
        /// <summary>
        /// Handles the specified command instance.
        /// </summary>
        /// <param name="command">The command instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command"/> argument is null.</exception>
        TResponse HandleCommand(TCommand command);
    }
}