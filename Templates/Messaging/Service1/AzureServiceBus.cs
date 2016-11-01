using System;
using System.Configuration;
using MassTransit;
using MassTransit.AzureServiceBusTransport;
using Microsoft.ServiceBus;

namespace Communication.Service1
{
    public class AzureServiceBus
    {
        public static IBusControl Start()
        {
            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], ConfigurationManager.AppSettings["AzureSbPath"]);

            var bus = Bus.Factory.CreateUsingAzureServiceBus(configuration =>
            {
                var host = configuration.Host(serviceUri, h =>
                {
                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["AzureSbKeyName"], ConfigurationManager.AppSettings["AzureSbSharedAccessKey"], TimeSpan.FromDays(1), TokenScope.Namespace);
                });
            });

            Console.WriteLine("Starting bus...");

            bus.Start();

            Console.WriteLine("Bus started.");

            Console.WriteLine();

            return bus;
        }
    }
}
