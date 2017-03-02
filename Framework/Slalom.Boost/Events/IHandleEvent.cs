using Slalom.Boost.Commands;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines a contract for handling events of the specified type.
    /// </summary>
    /// <typeparam name="TEvent">The type of event.</typeparam>
    /// <seealso cref="Event"/>
    public interface IHandleEvent<in TEvent> where TEvent : Event
    {
        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        /// <param name="context">The current <see cref="CommandContext"/> instance.</param>
        void Handle(TEvent instance, CommandContext context);
    }

    /// <summary>
    /// Defines a contract for handling all events.
    /// </summary>
    public interface IHandleEvent
    {
        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        /// <param name="context">The current <see cref="CommandContext"/> instance.</param>
        void Handle(Event instance, CommandContext context);
    }
}