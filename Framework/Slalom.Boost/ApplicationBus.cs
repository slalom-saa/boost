using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Autofac;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost
{
    /// <summary>
    /// Provides an in-process <see cref="IApplicationBus" /> implementation.
    /// </summary>
    public class ApplicationBus : IApplicationBus
    {
        private readonly IComponentContext _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationBus" /> class.
        /// </summary>
        /// <param name="container">The container to use.</param>
        public ApplicationBus(IComponentContext container)
        {
            _container = container;
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance)
        {
            var execution = _container.Resolve<IExecutionContextResolver>().Resolve();
            var context = new CommandContext(execution, CancellationToken.None);

            return this.Send(instance, context);
        }

        /// <summary>
        /// Sends the specified command along with a previously created context.  Used to call a command from within an execution chain.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context" /> argument is null.</exception>
        public Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CommandContext context)
        {
            var coordinator = _container.Resolve<ICommandCoordinator>();

            return coordinator.Handle(instance, context);
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CancellationToken cancellationToken)
        {
            var execution = _container.Resolve<IExecutionContextResolver>().Resolve();
            var context = new CommandContext(execution, cancellationToken);

            return this.Send(instance, context);
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="instance">The event to publish.</param>
        /// <param name="context">The current context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public Task Publish(IEvent instance, CommandContext context)
        {
            var publisher = _container.Resolve<IEventPublisher>();

            return publisher.Publish(instance, context);
        }

        /// <summary>
        /// Publishes the specified event.
        /// </summary>
        /// <param name="instance">The event to publish.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public Task Publish(IEvent instance)
        {
            return this.Publish(instance, null);
        }

        /// <summary>
        /// Publishes the specified events.
        /// </summary>
        /// <param name="instances">The events to publish.</param>
        /// <param name="context">The current context.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances" /> argument is null.</exception>
        public Task Publish(IEnumerable<IEvent> instances, CommandContext context)
        {
            var target = new List<Task>();
            foreach (var item in instances)
            {
                target.Add(this.Publish(item, context));
            }
            return Task.WhenAll(target);
        }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <param name="instance">The command to execute.</param>
        /// <returns>Returns a task for asynchronous programming.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        public async Task<CommandResult> Send(ICommand instance)
        {
            var execution = _container.Resolve<IExecutionContextResolver>().Resolve();
            var context = new CommandContext(execution, CancellationToken.None);

            var coordinator = _container.Resolve<ICommandCoordinator>();

            var result = await coordinator.Handle((dynamic) instance, context);

            return result as CommandResult;
        }
    }
}