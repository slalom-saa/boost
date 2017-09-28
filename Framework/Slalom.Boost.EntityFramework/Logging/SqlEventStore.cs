using System;
using System.Data.Entity;
using System.Linq;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost.EntityFramework.Logging
{
    public abstract class SqlEventStore : IEventStore
    {
        private readonly LoggingDbContext _context;

        protected SqlEventStore(LoggingDbContext context)
        {
            _context = context;
        }

        public virtual bool CanRead { get; } = true;

        public virtual void Append(Event instance, CommandContext context)
        {
            var source = new EventAudit(instance, context);
            _context.Events.Add(new EventEntryItem
            {
                TimeStamp = source.TimeStamp,
                RequestId = source.CommandId.ToString("D"),
                ApplicationName = source.Application,
                Environment = source.Environment,
                Event = source.Payload,
                EventId = Guid.NewGuid().ToString("D"),
                EventType = source.EventType,
                Name = source.EventName
            });

            _context.SaveChanges();
        }

        public virtual IQueryable<EventAudit> Find()
        {
            return _context.Set<EventAudit>();
        }

        public virtual void Handle(Event instance, CommandContext context)
        {
            this.Append(instance, context);
        }
    }
}
