using System;
using System.Configuration;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB.Aspects
{
    /// <summary>
    /// Provides a MongoDB based <see cref="IAuditStore"/> implementation.
    /// </summary>
    /// <seealso cref="IAuditStore" />
    public abstract class MongoAuditStore : IAuditStore
    {
        private readonly Lazy<IMongoCollection<CommandAudit>> _collection;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoAuditStore"/> class.
        /// </summary>
        protected MongoAuditStore()
            : this(ConfigurationManager.AppSettings["mongo:Database"])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoEventStore" /> class.
        /// </summary>
        /// <param name="database">The name of the database.</param>
        protected MongoAuditStore(string database)
        {
            _collection = new Lazy<IMongoCollection<CommandAudit>>(() => (this.Factory ?? new MongoConnectionFactory()).GetCollection<CommandAudit>(database, "Audit"));

            MongoMappings.EnsureInitialized(this);
        }

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
        /// Finds this instance.
        /// </summary>
        /// <returns>IQueryable&lt;CommandAudit&gt;.</returns>
        public IQueryable<CommandAudit> Find()
        {
            return _collection.Value.AsQueryable();
        }

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public bool CanRead { get; } = true;

        /// <summary>
        /// Saves the executed <see cref="T:Slalom.Boost.Commands.Command">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result">The <see cref="T:Slalom.Boost.Commands.CommandResult">command execution result</see>.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that saves the executed <see cref="T:Slalom.Boost.Commands.Command">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.</returns>
        public Task SaveAsync<TResponse>(Commands.Command<TResponse> command, CommandResult<TResponse> result)
        {
            return _collection.Value.InsertOneAsync(new CommandAudit(command, result));
        }
    }
}