using System;
using System.Collections.Generic;
using System.Configuration;
using Communication.Client;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;
using Slalom.Boost.RabbitMq;
using Slalom.Boost.RuntimeBinding;

namespace Communication.Service1
{
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class ServiceBus : RabbitServiceBus
    {
        protected override void ConfigureReceiveEndpoints(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
            base.ConfigureReceiveEndpoints(configurator, host);

            this.ConfigureCommandsEndpoint(configurator, host);
            this.ConfigureEventsEndpoint(configurator, host);
        }
    }
}