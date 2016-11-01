using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Domain.Events;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    public interface IHandledCommandData
    {
        /// <summary>
        /// Gets or sets the data that is a result of the command.
        /// </summary>
        /// <value>
        /// The data that is a result of the command.
        /// </value>
        dynamic Data { get; set; }

        /// <summary>
        /// Gets the events that were raised while handling the command.
        /// </summary>
        /// <value>
        /// The events that were raised while handling the command.
        /// </value>
        IEnumerable<Event> RaisedEvents { get; }

        /// <summary>
        /// Gets the validation messages that occurred while handling the command.
        /// </summary>
        /// <value>
        /// The validation messages that occurred while handling the command.
        /// </value>
        IEnumerable<ValidationMessage> ValidationMessages { get; }

        /// <summary>
        /// Adds the events to the result.
        /// </summary>
        /// <param name="events">The events to add.</param>
        void AddRaisedEvents(params Event[] events);

        /// <summary>
        /// Adds the validation messages to the result.
        /// </summary>
        /// <param name="messages">The messages to add.</param>
        void AddValidationMessages(params ValidationMessage[] messages);

        void SuppressRaisedEvents();
    }

    public class HandledCommandData : IHandledCommandData
    {
        private readonly List<Event> _events = new List<Event>();
        private readonly List<ValidationMessage> _messages = new List<ValidationMessage>();

        internal HandledCommandData()
        {
        }

        /// <summary>
        /// Gets or sets the data that is a result of the command.
        /// </summary>
        /// <value>
        /// The data that is a result of the command.
        /// </value>
        public dynamic Data { get; set; }

        /// <summary>
        /// Gets the events that were raised while handling the command.
        /// </summary>
        /// <value>
        /// The events that were raised while handling the command.
        /// </value>
        public IEnumerable<Event> RaisedEvents => _events.AsEnumerable();

        /// <summary>
        /// Gets the validation messages that occurred while handling the command.
        /// </summary>
        /// <value>
        /// The validation messages that occurred while handling the command.
        /// </value>
        public IEnumerable<ValidationMessage> ValidationMessages => _messages.AsEnumerable();

        /// <summary>
        /// Adds the events to the result.
        /// </summary>
        /// <param name="events">The events to add.</param>
        public void AddRaisedEvents(params Event[] events)
        {
            _events.AddRange(events);
        }

        /// <summary>
        /// Adds the validation messages to the result.
        /// </summary>
        /// <param name="messages">The messages to add.</param>
        public void AddValidationMessages(params ValidationMessage[] messages)
        {
            _messages.AddRange(messages);
        }

        public void SuppressRaisedEvents()
        {
            _events.Clear();
        }
    }
}