using System;
using System.Linq;
using MassTransit;
using Newtonsoft.Json;
using Slalom.Boost.Events;
using Slalom.Boost.ReadModel;

namespace Slalom.Boost.Commands
{
    public class EventAudit : IReadModelElement
    {
        public Guid CommandId { get; set; }

        public EventAudit()
        {
        }

        public EventAudit(IEvent @event, CommandContext context)
        {
            this.TimeStamp = @event.TimeStamp;
            this.EventName = @event.EventName;
            this.EventId = @event.Id;
            this.UserName = context.UserName;
            this.CorrelationId = context.CorrelationId;
            this.MachineName = context.MachineName;
            this.Application = context.Application;
            this.Session = context.Session;
            this.CommandId = context.CommandTrace.FirstOrDefault();
            this.Payload = JsonConvert.SerializeObject(@event, new JsonSerializerSettings
            {
                ContractResolver = new JsonCommandContractResolver()
            });
        }

        public string MachineName { get; set; }

        public string Application { get; set; }

        public Guid CorrelationId { get; set; }

        public string Session { get; set; }

        public string Payload { get; set; }

        public string EventName { get; set; }

        public Guid EventId { get; set; }

        public string UserName { get; set; }

        public DateTimeOffset TimeStamp { get; set; }

        public Guid Id { get; set; } = NewId.NextGuid();
    }
}