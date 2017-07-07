using System.Collections;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines an interface for resolving <see cref="IHandleEvent{TEvent}" /> instances.
    /// </summary>
    /// <seealso cref="IHandleEvent{TEvent}" />
    public interface IEventHandlerResolver
    {
        /// <summary>
        /// Resolves all event handlers for the specified type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="instance">The instance to find handlers for.</param>
        /// <returns>Returns all event handlers for the specified type.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        IEnumerable Resolve<TEvent>(TEvent instance) where TEvent : IEvent;
    }
}