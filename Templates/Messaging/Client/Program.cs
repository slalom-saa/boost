using System;
using Newtonsoft.Json;
using Slalom.Boost.RuntimeBinding;

namespace Communication.Client
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Client";

            try
            {
                using (var container = new ApplicationContainer(typeof(Program)))
                {
                    Console.WriteLine("Wait a few seconds and press any key to send commands...");
                    Console.ReadKey();

                    Console.WriteLine("Sending...");

                    var result = container.Bus.Send(new TestCommand("Command 1")).Result;

                    Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                    result = container.Bus.Send(new TestCommand("Command 2")).Result;

                    Console.WriteLine(JsonConvert.SerializeObject(result, Formatting.Indented));

                    Console.ReadKey();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

            Console.WriteLine("...");
        }
    }
}