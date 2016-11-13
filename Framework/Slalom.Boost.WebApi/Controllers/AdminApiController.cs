using System;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.WebApi.Controllers
{
    public abstract class AdminApiController : CommandApiController
    {
        [RuntimeBindingDependency]
        public IAuditStore[] AuditStores { get; set; }

        [RuntimeBindingDependency]
        public IEventStore[] EventStores { get; set; }

        protected IEnumerable<IQueryable<CommandAudit>> GetAuditQueries()
        {
            foreach (var item in this.AuditStores.Where(e => e.CanRead))
            {
                yield return item.Find()
                                 .OrderByDescending(e => e.TimeStamp)
                                 .Take(10);
            }
        }

        //protected IEnumerable<IQueryable<Event>> GetEventQueries()
        //{
        //    foreach (var item in this.EventStores.Where(e => e.CanRead))
        //    {
        //        yield return item.Find()
        //                         .OrderByDescending(e => e.TimeStamp)
        //                         .Take(10);
        //    }
        //}
    }
}