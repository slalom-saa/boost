using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using Newtonsoft.Json;
using Slalom.Boost.Events;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Contains all information about a completed command execution.
    /// </summary>
    [Serializable]
    public abstract class CommandResult
    {
        private readonly List<ValidationMessage> _validationMessages = new List<ValidationMessage>();

        /// <summary>
        /// Gets the command payload.
        /// </summary>
        /// <value>The command payload.</value>
        public string CommandPayload { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether command processing is canceled.
        /// </summary>
        /// <value><c>true</c> if canceled; otherwise, <c>false</c>.</value>
        public bool Canceled => this.Context.CancellationToken.IsCancellationRequested;

        /// <summary>
        /// Gets or sets the command identifier.
        /// </summary>
        /// <value>The command identifier.</value>
        public Guid CommandId { get; protected set; }

        /// <summary>
        /// Gets or sets the name of the command.
        /// </summary>
        /// <value>The name of the command.</value>
        public string CommandName { get; protected set; }

        /// <summary>
        /// Gets the completed date and time.
        /// </summary>
        /// <value>The completed date and time.</value>
        public DateTime? Completed { get; protected set; }

        /// <summary>
        /// Gets or sets the command context.
        /// </summary>
        /// <value>The command context.</value>
        public CommandContext Context { get; protected set; }

        /// <summary>
        /// Gets the time that it took to process the command from start to end.
        /// </summary>
        /// <value>The time that it took to process the command from start to end.</value>
        public TimeSpan Elapsed => this.Completed.HasValue ? (this.Completed.Value - this.Started) : TimeSpan.Zero;

        /// <summary>
        /// Gets the exception that was raised, if any.
        /// </summary>
        /// <value>The exception that was raised, if any.</value>
        public Exception Exception { get; private set; }

        /// <summary>
        /// Gets or sets the date and time the processing started.
        /// </summary>
        /// <value>The date and time the processing started.</value>
        public DateTime Started { get; protected set; }

        /// <summary>
        /// Gets a value indicating whether the command processing was successful.
        /// </summary>
        /// <value><c>true</c> if successful; otherwise, <c>false</c>.</value>
        public bool Successful => !this.ValidationMessages.Any() && this.Exception == null && this.Completed.HasValue;

        /// <summary>
        /// Gets any validation messages that were raised throughout the execution path.
        /// </summary>
        /// <value>The validation messages.</value>
        public IEnumerable<ValidationMessage> ValidationMessages => _validationMessages.AsEnumerable();

        /// <summary>
        /// Adds the specified validation messages to the result.
        /// </summary>
        /// <param name="messages">The messages to add.</param>
        public void AddValidationMessages(IEnumerable<ValidationMessage> messages)
        {
            _validationMessages.AddRange(messages.ToArray());
        }

        /// <summary>
        /// Adds the specified validation messages to the result.
        /// </summary>
        /// <param name="messages">The messages to add.</param>
        public void AddValidationMessages(params ValidationMessage[] messages)
        {
            _validationMessages.AddRange(messages);
        }

        /// <summary>
        /// Sets the exception that was raised.
        /// </summary>
        /// <param name="exception">The exception that was raised.</param>
        public void SetException(Exception exception)
        {
            this.Exception = exception;
            this.Context.AddException(exception);
        }
    }

    /// <summary>
    /// Contains all information about a completed command execution.
    /// </summary>
    /// <typeparam name="TResponse">The type of command response.</typeparam>
    /// <seealso cref="Slalom.Boost.Commands.CommandResult" />
    [Serializable]
    public class CommandResult<TResponse> : CommandResult
    {
        /// <summary>
        /// Gets or sets the command response.
        /// </summary>
        /// <value>The command response.</value>
        public TResponse Response { get; set; }

        /// <summary>
        /// Completes this instance.
        /// </summary>
        public void Complete()
        {
            this.Completed = DateTime.Now;
            if (this.Response != null && this.Response is IEvent)
            {
                this.Context.AddRaisedEvents((IEvent)this.Response);
            }
        }

        /// <summary>
        /// Starts the result tracking.
        /// </summary>
        /// <param name="command">The current command.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns the created command result instance.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="command"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> argument is null.</exception>
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static CommandResult<TResponse> Start(Command<TResponse> command, CommandContext context)
        {
            Argument.NotNull(() => command);
            Argument.NotNull(() => context);

            var target = new CommandResult<TResponse>
            {
                Started = DateTime.Now,
                Context = context,
                CommandId = command.Id,
                CommandName = command.GetType().Name,
                CommandPayload = JsonConvert.SerializeObject(command)
            };
            context.AddTrace(command);
            return target;
        }
    }
}