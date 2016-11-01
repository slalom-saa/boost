using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Slalom.Boost.Events;
using Slalom.Boost.ReadModel;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Represents the result and history of a command.
    /// </summary>
    public class CommandAudit : IReadModelElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAudit"/> class.
        /// </summary>
        public CommandAudit()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandAudit"/> class.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="result">The command result to build up the instance with.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="result"/> argument is null.</exception>
        public CommandAudit(ICommand command, CommandResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }

            this.TimeStamp = DateTime.Now;
            this.Canceled = result.Canceled;
            this.Completed = result.Completed;
            this.Elapsed = result.Elapsed;
            this.Exception = result.Exception?.ToString();
            this.Started = result.Started;
            this.Successful = result.Successful;
            this.CommandId = result.CommandId;
            this.CommandName = result.CommandName;
            this.CommandType = command.GetType().Name;
            var trace = result.Context.CommandTrace.ToList();
            var index = trace.IndexOf(this.CommandId);
            if (index > 0)
            {
                this.Predecessor = trace.ElementAt(index - 1);
            }
            var @event = ((dynamic)result).Response as IEvent;
            if (@event != null)
            {
                this.RaisedEvent = @event.EventName + "::" + @event.Id;
            }
            this.CommandPayload = JsonConvert.SerializeObject(command, new JsonSerializerSettings
            {
                ContractResolver = new JsonCommandContractResolver()
            });
            this.ValidationMessages = result.ValidationMessages;
            this.CorrelationId = result.Context.CorrelationId;
            this.Session = result.Context.Session;
            this.UserName = result.Context.UserName;
            this.AdditionalInformation = new Dictionary<string, string>(result.Context.AdditionalData);
        }

        /// <summary>
        /// Gets or sets the time stamp.
        /// </summary>
        /// <value>The time stamp.</value>
        public DateTime TimeStamp { get; set; }

        /// <summary>
        /// Gets the command payload.
        /// </summary>
        /// <value>The command payload.</value>
        public string CommandPayload { get; private set; }

        /// <summary>
        /// Gets or sets the additional context information.
        /// </summary>
        /// <value>The additional context information.</value>
        public Dictionary<string, string> AdditionalInformation { get; set; }

        /// <summary>
        /// Gets a value indicating whether command processing is canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled { get; set; }

        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; set; }

        /// <summary>
        /// Gets the completed date and time.
        /// </summary>
        /// <value>The completed date and time.</value>
        public DateTime? Completed { get; set; }

        /// <summary>
        /// Gets or sets the correlation identifier that can be used to track execution along a path.
        /// </summary>
        /// <value>The correlation identifier.</value>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Gets the time that it took to process the command from start to end.
        /// </summary>
        /// <value>The time that it took to process the command from start to end.</value>
        public TimeSpan Elapsed { get; set; }

        /// <summary>
        /// Gets the exception that was raised, if any.
        /// </summary>
        /// <value>The exception that was raised, if any.</value>
        public string Exception { get; set; }

        /// <summary>
        /// Gets or sets the preceding command identifier, if there was a preceding command.
        /// </summary>
        /// <value>The predecessor.</value>
        public Guid? Predecessor { get; set; }

        /// <summary>
        /// Gets the event that was raised as part of processing the command.
        /// </summary>
        /// <value>The raised event.</value>
        public string RaisedEvent { get; set; }

        /// <summary>
        /// Gets the execution session.
        /// </summary>
        /// <value>The session.</value>
        public string Session { get; set; }

        /// <summary>
        /// Gets or sets the date and time the processing started.
        /// </summary>
        /// <value>The date and time the processing started.</value>
        public DateTime Started { get; set; }

        /// <summary>
        /// Gets a value indicating whether the command processing was successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public bool Successful { get; set; }

        /// <summary>
        /// Gets the name of the actor initiating the command.
        /// </summary>
        /// <value>The name of the actor initiating the command.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets any validation messages that were raised throughout the execution path.
        /// </summary>
        /// <value>The validation messages.</value>
        public IEnumerable<ValidationMessage> ValidationMessages { get; set; }

        /// <summary>
        /// Gets or sets the type of the command.
        /// </summary>
        /// <value>The type of the command.</value>
        public string CommandType { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public Guid Id { get; set; } = Guid.NewGuid();

        /// <summary>
        /// Creates a dictionary from the instance.
        /// </summary>
        /// <returns>Returns a dictionary from the instance.</returns>
        public Dictionary<string, string> ToDictionary()
        {
            var target = new Dictionary<string, string>
            {
                { "Canceled", this.Canceled.ToString() },
                { "CommandId", this.CommandId.ToString() },
                { "CommandName", this.CommandName },
                { "CommandType", this.CommandType },
                { "Completed", this.Completed?.ToString() },
                { "CorrelationId", this.CorrelationId.ToString() },
                { "Elapsed", this.Elapsed.ToString() },
                { "Exception", this.Exception },
                { "Event", this.RaisedEvent },
                { "UserName", this.UserName },
                { "Successful", this.Successful.ToString() },
                { "CommandPayload", this.CommandPayload },
                { "Session", this.Session },
                { "ValidationMessages", string.Join("\n", this.ValidationMessages.Select(e => e.MessageType + ": " + e.Message)) }
            };
            foreach (var item in this.AdditionalInformation)
            {
                target.Add(item.Key, item.Value);
            }
            return target;
        }
    }
}