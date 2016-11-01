using System;
using System.Configuration;
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
            : this(ConfigurationManager.AppSettings["mongo:Database"])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEventStore"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        protected MongoEventStore(string database)
        {
            this.Collection = new Lazy<IMongoCollection<Event>>(() => (this.Factory ?? new MongoConnectionFactory()).GetCollection<Event>(database, "Log"));

            MongoMappings.EnsureInitialized(this);
        }

        /// <summary>
        /// Gets the collection factory.
        /// </summary>
        public Lazy<IMongoCollection<Event>> Collection { get; }

        /// <summary>
        /// Gets or sets the current <see cref="IMongoConnectionFactory"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMongoConnectionFactory"/> instance.</value>
        [RuntimeBindingDependency]
        public IMongoConnectionFactory Factory { get; set; }

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
            this.Collection.Value.InsertOne(instance);
        }

        /// <summary>
        /// Finds all <see cref="T:Slalom.Boost.Events.Event" /> instances.
        /// </summary>
        /// <returns>An IQueryable&lt;Event&gt; that can be used to filter and project.</returns>
        public IQueryable<Event> Find()
        {
            return this.Collection.Value.AsQueryable();
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