//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Slalom.Boost.Commands;
//using Slalom.Boost.Events;

//namespace Slalom.Boost.EntityFramework.Aspects
//{
//    public abstract class EntityFrameworkEventStore : IEventStore
//    {
//        private readonly DbContext _context;

//        protected EntityFrameworkEventStore(DbContext context)
//        {
//            _context = context;
//        }

//        public virtual bool CanRead { get; } = true;

//        public virtual void Append(Event instance, CommandContext context)
//        {
//            _context.Set<EventAudit>().Add(new EventAudit(instance, context));
//            _context.SaveChanges();
//        }

//        public virtual IQueryable<EventAudit> Find()
//        {
//            return _context.Set<EventAudit>();
//        }

//        public virtual void Handle(Event instance, CommandContext context)
//        {
//            this.Append(instance, context);
//        }
//    }
//}
