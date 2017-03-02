using System.Threading.Tasks;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines a contract for an event forwarder that forwards events across boundaries.
    /// </summary>
    public interface IEventForwarder
    {
        /// <summary>
        /// Forwards the specified event.
        /// </summary>
        /// <param name="instance">The event to forward.</param>
        /// <param name="context">The current context.</param>
        /// <returns>A task for asynchronous programming.</returns>
        Task Forward(IEvent instance, CommandContext context);
    }
}