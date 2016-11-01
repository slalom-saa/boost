using System;
using Slalom.Boost.RabbitMq;
using Slalom.Boost.RuntimeBinding;

namespace Communication.Client
{
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class ServiceBus : RabbitServiceBus
    {
    }
}