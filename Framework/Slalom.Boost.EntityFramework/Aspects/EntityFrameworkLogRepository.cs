using System;
using System.Data.Entity;
using System.Linq;
using Slalom.Boost.Aspects;

namespace Slalom.Boost.EntityFramework.Aspects
{
    /// <summary>
    /// A SQL <see cref="ILogRepository"/> implementation.
    /// </summary>
    /// <seealso cref="ILogRepository" />
    public abstract class EntityFrameworkLogRepository : ILogRepository
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkLogRepository" /> class.
        /// </summary>
        protected EntityFrameworkLogRepository(DbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Writes the specified message to the log.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="correlationId">The correlation identifier.</param>
        /// <param name="severity">The message severity.</param>
        /// <param name="data">Additional data to write to the log.</param>
        public void Add(object message, Guid correlationId, LogSeverity severity, params object[] data)
        {
            _context.Set<LogEntry>().Add(new LogEntry(message, data, correlationId, severity));
        }

        /// <summary>
        /// Finds all <see cref="T:Slalom.Boost.Aspects.LogEntry" /> instances.
        /// </summary>
        /// <returns>An IQueryable&lt;LogEntry&gt; that can be used to filter and project.</returns>
        public IQueryable<LogEntry> Find()
        {
            return _context.Set<LogEntry>()
                           .OrderByDescending(e => e.Timestamp);
        }
    }
}