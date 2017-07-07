using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Configuration;
using Slalom.Boost.Logging;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// An application, in-process, <see cref="IEventPublisher"/> implementation that executes event handlers asynchronously.
    /// </summary>
    /// <seealso cref="Event"/>
    /// <seealso cref="IHandleEvent{TEvent}"/>
    /// <seealso cref="IEventHandlerResolver"/>
    public class EventPublisher : IEventPublisher
    {
        protected readonly IComponentContext Container;
        protected readonly IEventHandlerResolver Resolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventPublisher"/> class.
        /// </summary>
        /// <param name="resolver">The event handler resolver to use.</param>
        /// <param name="container">The current container.</param>
        public EventPublisher(IEventHandlerResolver resolver, IComponentContext container)
        {
            Resolver = resolver;
            Container = container;
        }

        /// <summary>
        /// Publishes the specified instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <param name="instance">The instance to publish.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Task.</returns>
        public virtual Task Publish<TEvent>(TEvent instance, CommandContext context) where TEvent : IEvent
        {
            this.HandleEvent(instance, context);

            return Task.FromResult(0);
        }

        /// <summary>
        /// Publishes the specified instances.
        /// </summary>
        /// <typeparam name="TEvent">The type of events</typeparam>
        /// <param name="instances">The instances to publish.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Task.</returns>
        public virtual Task Publish<TEvent>(IEnumerable<TEvent> instances, CommandContext context) where TEvent : IEvent
        {
            foreach (var instance in instances)
            {
                this.HandleEvent(instance, context);
            }

            return Task.FromResult(0);
        }

        /// <summary>
        /// Handles the specified event instance.
        /// </summary>
        /// <typeparam name="TEvent">The event type</typeparam>
        /// <param name="context">The current context.</param>
        /// <param name="instance">The event instance to handler.</param>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public virtual void HandleEvent<TEvent>(TEvent instance, CommandContext context) where TEvent : IEvent
        {
            foreach (var handler in Resolver.Resolve(instance))
            {
                try
                {
                    var method = handler.GetType().GetMethod("Handle", new[] { instance.GetType(), typeof(CommandContext) });
                    method.Invoke(handler, new object[] { instance, context });
                }
                catch (Exception exception)
                {
                    var logger = Container.Resolve<ILogger>();
                    logger.Error(exception, "An exception occurred while handling an event. {@Event} {@Context}", instance, context);
                    throw;
                }
            }

            try
            {
                foreach (var item in Container.ResolveAll<IEventForwarder>())
                {
                    item.Forward(instance, context);
                }
            }
            catch (Exception exception)
            {
                Trace.TraceError("An unhandled exception was raised when forwarding events.  " + exception);
            }
        }
    }
}