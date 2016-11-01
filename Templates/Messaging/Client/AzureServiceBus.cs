//using System;
//using System.Configuration;
//using MassTransit;
//using MassTransit.AzureServiceBusTransport;
//using Microsoft.ServiceBus;

//namespace Communication.Client
//{

//    public class AzureServiceBus
//    {
//        public static IBusControl Start()
//        {
//            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], ConfigurationManager.AppSettings["AzureSbPath"]);

//            var bus = Bus.Factory.CreateUsingAzureServiceBus(configuration =>
//            {
//                var host = configuration.Host(serviceUri, h =>
//                {
//                    h.TokenProvider = TokenProvider.CreateSharedAccessSignatureTokenProvider(ConfigurationManager.AppSettings["AzureSbKeyName"], ConfigurationManager.AppSettings["AzureSbSharedAccessKey"], TimeSpan.FromDays(1), TokenScope.Namespace);
//                });
//            });


//            Console.WriteLine("Starting bus...");

//            bus.Start();

//            Console.WriteLine("Bus started.");

//            Console.WriteLine();

//            return bus;
//        }

//        public static IRequestClient<T, U> CreateClient<T, U>(IBusControl bus) where U : class where T : class
//        {
//            var serviceUri = ServiceBusEnvironment.CreateServiceUri("sb", ConfigurationManager.AppSettings["AzureSbNamespace"], ConfigurationManager.AppSettings["AzureSbPath"]);

//            var client = bus.CreateRequestClient<T, U>(new Uri(serviceUri, ConfigurationManager.AppSettings["ServiceQueueName"]), TimeSpan.FromSeconds(30));

//            return client;
//        }
//    }
//}
