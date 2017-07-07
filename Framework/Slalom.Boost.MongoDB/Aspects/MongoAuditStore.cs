using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Driver;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;

namespace Slalom.Boost.MongoDB.Aspects
{
    /// <summary>
    /// Provides a MongoDB based <see cref="IAuditStore"/> implementation.
    /// </summary>
    /// <seealso cref="IAuditStore" />
    public abstract class MongoAuditStore : IAuditStore
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoAuditStore"/> class.
        /// </summary>
        protected MongoAuditStore()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoAuditStore"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        protected MongoAuditStore(MongoDbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the current <see cref="MongoDbContext"/> instance.
        /// </summary>
        /// <value>The current <see cref="MongoDbContext"/> instance.</value>
        public MongoDbContext Context { get; set; }

        /// <summary>
        /// Gets or sets the current <see cref="IMapper"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMapper"/> instance.</value>
        public IMapper Mapper { get; set; }

        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public bool CanRead { get; } = true;

        /// <summary>
        /// Finds this instance.
        /// </summary>
        /// <returns>IQueryable&lt;CommandAudit&gt;.</returns>
        public IQueryable<CommandAudit> Find()
        {
            return this.Context.GetCollection<CommandAudit>().AsQueryable();
        }

        /// <summary>
        /// Saves the executed <see cref="T:Slalom.Boost.Commands.Command">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result">The <see cref="T:Slalom.Boost.Commands.CommandResult">command execution result</see>.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that saves the executed <see cref="T:Slalom.Boost.Commands.Command">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.</returns>
        public Task SaveAsync<TResponse>(Commands.Command<TResponse> command, CommandResult<TResponse> result)
        {
            return this.Context.GetCollection<CommandAudit>().InsertOneAsync(new CommandAudit(command, result));
        }
    }
}