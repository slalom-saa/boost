using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost
{
    /// <summary>
    /// Defines a Service Bus for inter-application communication and integration.  Similarly, a <see cref="ApplicationBus"/> defines intra-application communication. 
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IServiceBus : IDisposable
    {
        Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CommandContext context);

        Task Publish(IEvent instance, CommandContext context);

        Task Publish(IEnumerable<IEvent> instances, CommandContext context);
    }
}