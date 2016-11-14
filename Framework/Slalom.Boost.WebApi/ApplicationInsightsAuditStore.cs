using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Slalom.Boost.Commands;

namespace Slalom.Boost.WebApi
{
    /// <summary>
    /// Provides an Application Insights <see cref="IAuditStore"/>.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Commands.IAuditStore" />
    public abstract class ApplicationInsightsAuditStore : IAuditStore
    {
        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public virtual bool CanRead { get; }

        /// <summary>
        /// Saves the executed <see cref="T:Slalom.Boost.Commands.Command`1">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.
        /// </summary>
        /// <typeparam name="TResponse">The type of the t response.</typeparam>
        /// <param name="command">The executing command.</param>
        /// <param name="result">The <see cref="T:Slalom.Boost.Commands.CommandResult">command execution result</see>.</param>
        /// <returns>A <see cref="T:System.Threading.Tasks.Task" /> that saves the executed <see cref="T:Slalom.Boost.Commands.Command`1">command</see> and <see cref="T:Slalom.Boost.Commands.CommandResult">result</see>.</returns>
        public virtual Task SaveAsync<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            var model = new CommandAudit(command, result);
            var telemetry = new TelemetryClient();
            telemetry.TrackEvent("Audit: " + result.CommandName, model.ToDictionary());
            telemetry.Flush();
            return Task.FromResult(0);
        }

        /// <summary>
        /// Finds all <see cref="T:Slalom.Boost.Commands.CommandAudit" /> instances.
        /// </summary>
        /// <returns>IQueryable&lt;CommandAudit&gt;.</returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public virtual IQueryable<CommandAudit> Find()
        {
            throw new NotSupportedException();
        }
    }
}