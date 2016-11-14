using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost.WebApi
{
    /// <summary>
    /// Provides an Application Insights <see cref="IEventStore" />.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Events.IEventStore" />
    /// <seealso cref="Slalom.Boost.Events.IHandleEvent" />
    public abstract class ApplicationInsightsEventStore : IEventStore
    {
        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        public virtual bool CanRead { get; }

        /// <summary>
        /// Appends the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        public virtual void Append(Event instance, CommandContext context)
        {
            var telemetry = new TelemetryClient();

            var dictionary = new Dictionary<string, string>
            {
                { "EventId", instance.Id.ToString() },
                { "EventName", instance.EventName },
                { "EventType", instance.EventType.ToString() },
                { "TimeStamp", instance.TimeStamp.ToString(CultureInfo.InvariantCulture) },
                { "CorrelationId", context?.CorrelationId.ToString() },
                {
                    "Payload", JsonConvert.SerializeObject(instance, new JsonSerializerSettings
                    {
                        ContractResolver = new JsonEventContractResolver()
                    })
                }
            };

            telemetry.TrackEvent("Event: " + instance.EventName, dictionary);

            telemetry.Flush();
        }

        /// <summary>
        /// Finds all <see cref="T:Slalom.Boost.Events.Event" /> instances.
        /// </summary>
        /// <returns>An IQueryable&lt;Event&gt; that can be used to filter and project.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public virtual IQueryable<EventAudit> Find()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <param name="instance">The event instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        public virtual void Handle(Event instance, CommandContext context)
        {
            this.Append(instance, context);
        }
    }
}