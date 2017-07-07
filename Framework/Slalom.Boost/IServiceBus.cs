using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost
{
    /// <summary>
    /// Defines a Service Bus for inter-application communication and integration.  Similarly, a <see cref="ApplicationBus" /> defines intra-application communication.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    public interface IServiceBus : IDisposable
    {
        Task Publish(IEvent instance, CommandContext context);

        Task Publish(IEnumerable<IEvent> instances, CommandContext context);
        Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CommandContext context);
    }
}