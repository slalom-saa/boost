using System;
using System.Diagnostics.CodeAnalysis;
using Newtonsoft.Json;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Represents a Domain Event.
    /// </summary>
    /// <seealso cref="IEventPublisher"/>
    /// <seealso cref="IHandleEvent{TEvent}"/>
    [Serializable]
    public abstract class Event : IEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        protected Event()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Event"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        protected Event(Guid id)
        {
            this.Id = id;
        }

        /// <summary>
        /// Gets the event ID.
        /// </summary>
        /// <value>The event ID.</value>
        public Guid Id { get; private set; } = Guid.NewGuid();

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public Type EventType => this.GetType();

        /// <summary>
        /// Gets the name of the event.
        /// </summary>
        /// <value>The name of the event.</value>
        public string EventName => this.GetType().Name;

        /// <summary>
        /// Gets the time stamp of when the event was created.
        /// </summary>
        /// <value>The time stamp of when the event was created.</value>
        public DateTime TimeStamp { get; } = DateTime.Now;

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            if (obj.GetType() != this.GetType())
            {
                return false;
            }
            return this.Equals((Event)obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        [SuppressMessage("ReSharper", "NonReadonlyMemberInGetHashCode")]
        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        /// <summary>
        /// Determines if another event instance is equal to this instance.
        /// </summary>
        /// <param name="other">The instance to compare.</param>
        /// <returns><c>true</c> if the instances are equal; <c>false</c> otherwise.</returns>
        protected bool Equals(Event other)
        {
            return other != null && this.Id.Equals(other.Id);
        }
    }
}