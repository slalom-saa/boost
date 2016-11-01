using System.Collections.Generic;
using System.Threading.Tasks;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines an <see href="https://www.safaribooksonline.com/library/view/implementing-domain-driven-design/9780133039900/ch08lev1sec1.html">Event Publisher</see>, responsible for locating event handlers and executing multi-threaded and/or out-of-process flow.
    /// </summary>
    /// <seealso cref="Event"/>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IEventPublisher
    {
        /// <summary>
        /// Publishes the specified instance.
        /// </summary>
        /// <typeparam name="TEvent">The type of event.</typeparam>
        /// <param name="context">The current context.</param>
        /// <param name="instance">The instance to publish.</param>
        Task Publish<TEvent>(TEvent instance, CommandContext context) where TEvent : IEvent;

        /// <summary>
        /// Publishes the specified instances.
        /// </summary>
        /// <typeparam name="TEvent">The type of events</typeparam>
        /// <param name="context">The current context.</param>
        /// <param name="instances">The instances to publish.</param>
        Task Publish<TEvent>(IEnumerable<TEvent> instances, CommandContext context) where TEvent : IEvent;
    }
}