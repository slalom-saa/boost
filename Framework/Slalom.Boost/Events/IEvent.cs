using System;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines a common interface for events.
    /// </summary>
    public interface IEvent
    {
        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        string EventName { get; }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        Type EventType { get; }

        /// <summary>
        /// Gets the event ID.
        /// </summary>
        /// <value>The event ID.</value>
        Guid Id { get; }

        /// <summary>
        /// Gets the time stamp of when the event was created.
        /// </summary>
        /// <value>The time stamp of when the event was created.</value>
        DateTime TimeStamp { get; }
    }
}