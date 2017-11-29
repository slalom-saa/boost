using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Threading;
using Newtonsoft.Json;
using Slalom.Boost.Events;
using Slalom.Boost.Serialization;
using ExecutionContext = Slalom.Boost.Aspects.ExecutionContext;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Provides context for a command which enables multi-threaded/multi-process execution.
    /// </summary>
    /// <remarks>
    /// This class is intended to be serialized to JSON in order to persist with the command.
    /// </remarks>
    [Serializable]
    public class CommandContext
    {
        [NonSerialized]
        private readonly CancellationToken _cancellationToken;

        private readonly ConcurrentDictionary<string, object> _executionData = new ConcurrentDictionary<string, object>();

        [NonSerialized]
        private readonly IIdentity _identity;

        private readonly List<IEvent> _raisedEvents = new List<IEvent>();

        private readonly List<Exception> _raisedExceptions = new List<Exception>();

        private readonly List<Guid> _trace = new List<Guid>();

        public string SourceAddress { get; set; }

        public string Url { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext"/> class.
        /// </summary>
        [JsonConstructor]
        protected CommandContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandContext" /> class.
        /// </summary>
        /// <param name="context">The current execution context.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <exception cref="System.ArgumentNullException">Thrown when the <paramref name="cancellationToken" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context" /> argument is null.</exception>
        public CommandContext(ExecutionContext context, string commandName, Guid commandId, CancellationToken cancellationToken)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
          

            _cancellationToken = cancellationToken;
            _identity = context.Identity;
            this.UserName = context.Identity?.Name ?? "Anonymous";
            this.Session = context.Session;
            this.Created = DateTime.Now;
            this.AdditionalData = new ReadOnlyDictionary<string, string>(context.Data);
            this.CorrelationId = context.CorrelationId;
            this.Application = context.Application;
            this.MachineName = context.MachineName;
            this.Environment = context.Environment;
            this.Version = context.Version;
            this.Build = context.Build;
            this.CommandName = commandName;
            this.CommandId = commandId;
            this.SourceAddress = context.SourceAddress;
            this.Url = context.Url;
        }

        public string Build { get; set; }

        public string Version { get; set; }

        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the cancellation token.
        /// </summary>
        /// <value>The cancellation token.</value>
        [Ignore]
        public CancellationToken CancellationToken => _cancellationToken;

        /// <summary>
        /// Gets any additional data about the context.
        /// </summary>
        public ReadOnlyDictionary<string, string> AdditionalData { get; private set; }

        /// <summary>
        /// Gets the command identifier trace.
        /// </summary>
        /// <value>The command identifier trace.</value>
        public IEnumerable<Guid> CommandTrace => _trace.AsEnumerable();

        /// <summary>
        /// Gets the correlation identifier.
        /// </summary>
        /// <value>The correlation identifier.</value>
        public Guid CorrelationId { get; private set; }

        /// <summary>
        /// Gets the date and time the context was created.
        /// </summary>
        /// <remarks>
        /// This should be the same date and time that command was added to the command pipeline.
        /// </remarks>
        /// <value>The date and time the context was created.</value>
        public DateTime Created { get; private set; }

        /// <summary>
        /// Gets the identity executing the command.
        /// </summary>
        /// <value>The identity executing the command.</value>
        [Ignore]
        public IIdentity Identity => _identity;

        /// <summary>
        /// Provides access to a contextual, execution-based store.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <returns>The object stored with the specified key.</returns>
        public object this[string key]
        {
            get
            {
                object outValue = null;
                if (_executionData.TryGetValue(key, out outValue))
                {
                    return outValue;
                }
                return null;
            }
            set { _executionData.AddOrUpdate(key, a => value, (a, b) => value); }
        }

        /// <summary>
        /// Gets the events that were raised, but not part of the response.
        /// </summary>
        /// <value>The events that were raised, but not part of the response.</value>
        public IEnumerable<IEvent> RaisedEvents => _raisedEvents.AsEnumerable();

        /// <summary>
        /// Gets any exceptions that were raised throughout the execution path.
        /// </summary>
        /// <value>Any exceptions that were raised throughout the execution path.</value>
        public IEnumerable<Exception> RaisedExceptions => _raisedExceptions.AsEnumerable();

        /// <summary>
        /// Gets or sets the current user session key.
        /// </summary>
        /// <value>The current user session key.</value>
        public string Session { get; private set; }

        /// <summary>
        /// Gets the name of the actor initiating the command.
        /// </summary>
        /// <value>The name of the actor initiating the command.</value>
        public string UserName { get; private set; }

        /// <summary>
        /// Gets the application name.
        /// </summary>
        /// <value>The application name.</value>
        public string Application { get; private set; }

        /// <summary>
        /// Gets the name of the machine.
        /// </summary>
        /// <value>The name of the machine.</value>
        public string MachineName { get; private set; }

        public string CommandName { get; private set; }

        public Guid CommandId { get; private set; }

        /// <summary>
        /// Adds the raised events to the collection.
        /// </summary>
        /// <param name="instances">The instances to add.</param>
        public void AddRaisedEvents(params IEvent[] instances)
        {
            _raisedEvents.AddRange(instances);
        }

        /// <summary>
        /// Gets a strongly typed instance from the contextual, execution-based store.
        /// </summary>
        /// <typeparam name="TInstance">The type of item stored.</typeparam>
        /// <returns>The object stored with the specified type.</returns>
        public TInstance Get<TInstance>() where TInstance : class
        {
            return this.Get<TInstance>("__" + typeof(TInstance).Name);
        }

        /// <summary>
        /// Gets a strongly typed instance from the contextual, execution-based store.
        /// </summary>
        /// <typeparam name="TInstance">The type of item stored.</typeparam>
        /// <param name="key">The object key.</param>
        /// <returns>The object stored with the specified key.</returns>
        public TInstance Get<TInstance>(string key) where TInstance : class
        {
            return this[key] as TInstance;
        }

        /// <summary>
        /// Stores the instance with the specified key in the contextual, execution-based store.
        /// </summary>
        /// <param name="key">The object key.</param>
        /// <param name="value">The object.</param>
        /// <returns>The object stored with the specified key.</returns>
        public void Set(string key, object value)
        {
            this[key] = value;
        }

        /// <summary>
        /// Stores the instance of the specified type in the contextual, execution-based store.
        /// </summary>
        /// <param name="value">The instance to store.</param>
        /// <returns>The object stored of the specified type.</returns>
        public void Set<TInstance>(TInstance value)
        {
            this["__" + typeof(TInstance).Name] = value;
        }

        internal void AddException(Exception exception)
        {
            _raisedExceptions.Add(exception);
        }

        internal void AddTrace(ICommand command)
        {
            _trace.Add(command.Id);
        }
    }
}