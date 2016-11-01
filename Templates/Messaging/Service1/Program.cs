using System;
using Communication.Client;
using Slalom.Boost;
using Slalom.Boost.RuntimeBinding;

namespace Communication.Service1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Service 1";

            using (var container = new ApplicationContainer(typeof(Program)))
            {
                var bus = container.Resolve<IServiceBus>();

                Console.WriteLine("Waiting...");

                Console.ReadKey();
            }
        }
    }
}
