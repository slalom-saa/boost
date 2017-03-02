using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Slalom.Boost.Configuration;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// An <see cref="IContainer" /> based <see cref="IEventHandlerResolver" /> implementation.
    /// </summary>
    /// <seealso cref="IContainer"/>
    /// <seealso cref="IHandleEvent{TEvent}"/>
    public class EventHandlerResolver : IEventHandlerResolver
    {
        private readonly IComponentContext _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="EventHandlerResolver"/> class.
        /// </summary>
        /// <param name="container">The container to use.</param>
        public EventHandlerResolver(IComponentContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = container;
        }

        /// <summary>
        /// Resolves all event handlers for the specified type.
        /// </summary>
        /// <typeparam name="TEvent">The type of the event.</typeparam>
        /// <param name="instance">The instance to find handlers for.</param>
        /// <returns>Returns all event handlers for the specified type.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IEnumerable Resolve<TEvent>(TEvent instance) where TEvent : IEvent
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            var type = typeof(IHandleEvent<>).MakeGenericType(instance.GetType());

            var result = _container.ResolveAll(type).Concat(_container.ResolveAll<IHandleEvent>());

            return result;
        }
    }
}