using System;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Provides a base event handler implementation.
    /// </summary>
    /// <typeparam name="TEvent">The type of the t event.</typeparam>
    /// <seealso cref="Slalom.Boost.Events.IHandleEvent{TEvent}" />
    public abstract class EventHandler<TEvent> : IHandleEvent<TEvent> where TEvent : Event
    {
        /// <summary>
        /// Gets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <value>The current <see cref="CommandContext"/>.</value>
        public CommandContext Context { get; private set; }

        /// <summary>
        /// Gets the current <see cref="IDataFacade"/>.
        /// </summary>
        /// <value>The current <see cref="IDataFacade"/>.</value>
        public IDataFacade DataFacade { get; protected set; }

        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        public void Handle(TEvent instance, CommandContext context)
        {
            this.Context = context;

            this.Handle(instance);
        }

        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        public abstract void Handle(TEvent instance);
    }
}