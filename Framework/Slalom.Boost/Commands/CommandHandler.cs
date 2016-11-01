using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Provides a base for command handlers.
    /// </summary>
    /// <typeparam name="TCommand">The type of command.</typeparam>
    /// <typeparam name="TResponse">The type of response.</typeparam>
    /// <seealso cref="Slalom.Boost.Commands.IHandleCommand{TCommand, TResponse}" />
    public abstract class CommandHandler<TCommand, TResponse> : IHandleCommand<TCommand, TResponse> where TCommand : Command<TResponse>

    {
        [RuntimeBindingDependency]
        [SuppressMessage("ReSharper", "UnusedAutoPropertyAccessor.Local")]
        private IApplicationBus Bus { get; set; }

        /// <summary>
        /// Handles the specified command instance.
        /// </summary>
        /// <param name="command">The command instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command"/> argument is null.</exception>
        public abstract TResponse HandleCommand(TCommand command);

        /// <summary>
        /// Gets or sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <value>The current <see cref="CommandContext"/>.</value>
        public CommandContext Context { get; set; }

        /// <summary>
        /// Sets the current <see cref="CommandContext" />.
        /// </summary>
        /// <param name="context">The current <see cref="CommandContext" />.</param>
        public void SetContext(CommandContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IDataFacade"/> instance.
        /// </value>
        [RuntimeBindingDependency]
        public IDataFacade DataFacade { get; set; }

        /// <summary>
        /// Sends the specified command.
        /// </summary>
        /// <typeparam name="TChainedResponse">The expected response from the handling of the command.</typeparam>
        /// <param name="instance">The command to execute.</param>
        /// <returns>Returns a task that contains the result of the command.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance" /> argument is null.</exception>
        protected Task<CommandResult<TChainedResponse>> Send<TChainedResponse>(Command<TChainedResponse> instance)
        {
            return this.Bus.Send(instance, this.Context);
        }
    }
}