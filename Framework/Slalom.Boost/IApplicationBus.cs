using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost
{
    /// <summary>
    /// Defines a single point of entry for <see cref="Command{TResponse}">Commands</see> and<see cref="Event">Events</see>.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IApplicationBus
    {
        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CancellationToken cancellationToken);

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance);

        /// <summary>
        /// Sends the specified command along with a previously created context.  Used to call a command from within an execution chain.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context" /> argument is null.</exception>
        Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CommandContext context);

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="instance">The event to publish.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        Task Publish(IEvent instance);

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="instance">The event to publish.</param>
        /// <param name="context">The current context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        Task Publish(IEvent instance, CommandContext context);

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="instances">The events to publish.</param>
        /// <param name="context">The current context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        Task Publish(IEnumerable<IEvent> instances, CommandContext context);
    }
}