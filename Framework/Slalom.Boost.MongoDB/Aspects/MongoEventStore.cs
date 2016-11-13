using System;
using System.Linq;
using MongoDB.Driver;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB.Aspects
{
    /// <summary>
    /// A MongoDB <see cref="IEventStore"/> implementation.
    /// </summary>
    /// <seealso cref="IEventStore" />
    public abstract class MongoEventStore : IEventStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEventStore"/> class.
        /// </summary>
        protected MongoEventStore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEventStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected MongoEventStore(MongoDbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the current <see cref="MongoDbContext"/> instance.
        /// </summary>
        /// <value>The current <see cref="MongoDbContext"/> instance.</value>
        [RuntimeBindingDependency]
        public MongoDbContext Context { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="IMapper"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMapper"/> instance.</value>
        [RuntimeBindingDependency]
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public bool CanRead { get; } = true;

        /// <summary>
        /// Appends the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        public void Append(Event instance, CommandContext context)
        {
            this.Context.GetCollection<EventAudit>().InsertOne(new EventAudit(instance, context));
        }

        /// <summary>
        /// Finds all <see cref="T:Slalom.Boost.Events.Event" /> instances.
        /// </summary>
        /// <returns>An IQueryable&lt;Event&gt; that can be used to filter and project.</returns>
        public IQueryable<EventAudit> Find()
        {
            return this.Context.GetCollection<EventAudit>().AsQueryable();
        }

        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        public void Handle(Event instance, CommandContext context)
        {
            this.Append(instance, context);
        }
    }
}