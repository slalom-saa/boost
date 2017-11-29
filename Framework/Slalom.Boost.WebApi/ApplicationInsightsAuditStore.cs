using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
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
            var request = new RequestTelemetry(command.CommandType.Name, result.Started, result.Elapsed, GetStatusCode(result), result.Successful);

            request.Url = new Uri(result.Context.Url, UriKind.RelativeOrAbsolute);
            request.Context.UpdateContext(result.Context);

            if (result.ValidationMessages.Any())
            {
                request.Properties.Add("ValidationErrors.Count", result.ValidationMessages.Count().ToString());
                request.Properties.Add("ValidationErrors.Messages", String.Join("; ", result.ValidationMessages.Select(e => e.MessageType + ": " + e.Message)));
            }

            var telemetry = new TelemetryClient();
            telemetry.TrackRequest(request);


            if (result.Exception != null)
            {
                var exception = new ExceptionTelemetry(result.Exception);
                exception.Context.UpdateContext(result.Context);
                telemetry.TrackException(exception);
            }

            telemetry.Flush();

            return Task.FromResult(0);
        }

        private static string GetStatusCode<TResponse>(CommandResult<TResponse> result)
        {
            var status = "200";
            if (!result.Successful)
            {
                if (result.ValidationMessages.Any())
                {
                    status = result.ValidationMessages.Any(e => e.MessageType == Validation.ValidationMessageType.Security) ? "403" : "400";
                }
                else
                {
                    status = "500";
                }
            }
            return status;
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

    public static class ContextExtensions
    {
        public static void UpdateContext(this TelemetryContext source, CommandContext context)
        {
            source.User.Id = context.UserName;
            source.Session.Id = context.Session;
            source.Operation.Name = context.CommandName;
            source.Operation.Id = context.CommandId.ToString("N");
            source.Component.Version = context.Version;
            source.Location.Ip = context.SourceAddress;

            source.Properties.Add("CorrelationId", context.CorrelationId.ToString("N"));
            source.Properties.Add("Application", context.Application);
            source.Properties.Add("Build", context.Build);
            source.Properties.Add("Environment", context.Environment);
        }
    }
}