using System;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Domain.Events;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    public class CommandResponse
    {
        private readonly List<Event> _raisedEvents = new List<Event>();

        private readonly List<ValidationMessage> _validationMessages = new List<ValidationMessage>();

        public dynamic Data { get; set; }

        public IEnumerable<Event> RaisedEvents => _raisedEvents.AsEnumerable();

        public IEnumerable<ValidationMessage> ValidationMessages => _validationMessages.AsEnumerable();

        public Exception Exception { get; set; }

        public void AddRaisedEvents(IEnumerable<Event> events)
        {
            this.AddRaisedEvents(events.ToArray());
        }

        public void AddRaisedEvents(params Event[] events)
        {
            _raisedEvents.AddRange(events);
        }

        public void AddValidationMessages(IEnumerable<ValidationMessage> messages)
        {
            _validationMessages.AddRange(messages.ToArray());
        }

        public void AddValidationMessages(params ValidationMessage[] messages)
        {
            _validationMessages.AddRange(messages);
        }

        public void Update(IHandleCommand instance)
        {
            this.AddRaisedEvents(instance.RaisedEvents);
            this.AddValidationMessages(instance.ValidationMessages);
            this.Exception = instance.Exception;
            this.Data = instance.Data;
        }
    }
}