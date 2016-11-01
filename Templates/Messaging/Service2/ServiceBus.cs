using System;
using Communication.Client;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Slalom.Boost.RabbitMq;
using Slalom.Boost.RuntimeBinding;

namespace Communication.Service2
{
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class ServiceBus : RabbitServiceBus
    {
        protected override void ConfigureReceiveEndpoints(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
            base.ConfigureReceiveEndpoints(configurator, host);

            this.ConfigureEventsEndpoint(configurator, host);
        }
    }
}