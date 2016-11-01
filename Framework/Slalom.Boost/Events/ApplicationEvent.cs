using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Slalom.Boost.Aspects;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Provides a flattened read model for log entries.
    /// </summary>
    public class ApplicationEvent : Event
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent"/> class.
        /// </summary>
        protected ApplicationEvent()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationEvent" /> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="context">The context.</param>
        /// <param name="severity">The severity.</param>
        /// <param name="data">The data.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed.")]
        public ApplicationEvent(object message, ExecutionContext context, LogSeverity severity, params object[] data)
        {
            try
            {
                this.Message = (message as string) ?? JsonConvert.SerializeObject(message);
            }
            catch
            {
                this.Message = Convert.ToString(message, CultureInfo.InvariantCulture);
            }
            try
            {
                this.AdditionalData = JsonConvert.SerializeObject(data);
            }
            catch
            {
                this.AdditionalData = "Serialization failed.";
            }
            this.CorrelationId = context.CorrelationId;
            this.Timestamp = DateTime.Now;
            this.Severity = severity;
            this.Session = context.Session;
            this.UserName = context.Identity?.Name;
            this.ExecutionData = context.Data;
        }

        /// <summary>
        /// Gets or sets the execution data.
        /// </summary>
        /// <value>The execution data.</value>
        public Dictionary<string, string> ExecutionData { get; set; }

        /// <summary>
        /// Gets or sets the name of the user.
        /// </summary>
        /// <value>The name of the user.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the session.
        /// </summary>
        /// <value>The session.</value>
        public string Session { get; set; }

        /// <summary>
        /// Gets or sets the log entry data.
        /// </summary>
        /// <value>
        /// The log entry data.
        /// </value>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets the log entry message.
        /// </summary>
        /// <value>
        /// The log entry message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>The correlation identifier.</value>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets or sets the log entry severity.
        /// </summary>
        /// <value>
        /// The log entry severity.
        /// </value>
        [JsonConverter(typeof(StringEnumConverter))]
        public LogSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        public DateTime Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the entry identifier.
        /// </summary>
        /// <value>
        /// The entry identifier.
        /// </value>
        public Guid Id { get; set; } = Guid.NewGuid();
    }
}