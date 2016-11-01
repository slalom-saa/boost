using System.Threading.Tasks;
using Slalom.Boost;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Communication.Client
{
    public class ServiceBusEventForwarder : IEventForwarder
    {
        private readonly IServiceBus _bus;

        public ServiceBusEventForwarder(IServiceBus bus)
        {
            _bus = bus;
        }

        public Task Forward(IEvent instance, CommandContext context)
        {
            _bus.Publish(instance, context).Wait();

            return Task.FromResult(0);
        }
    }
}