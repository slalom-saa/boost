using System;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Slalom.Boost.Commands;
using Slalom.Boost.EntityFramework;
using Slalom.Boost.EntityFramework.Aspects;
using Slalom.Boost.Events;
using Slalom.Boost.Serialization;

#pragma warning disable 4014

namespace Slalom.Boost.UnitTests
{
    public class TestEvent : Event
    {
        public TestEvent(string content)
        {
            this.Content = content;
        }

        public string Content { get; }
    }

    public class TestCommand : Command<TestEvent>
    {
        public TestCommand(string content)
        {
            this.Content = content;
        }

        [Ignore]
        public string Content { get; }
    }

    public class TestCommandHandler : CommandHandler<TestCommand, TestEvent>
    {
        public override TestEvent HandleCommand(TestCommand command)
        {
            throw new Exception("...");
            return new TestEvent(command.Content);
        }
    }

    public class TestContext : BoostDbContext
    {
        public TestContext() : base("Name=Local")
        {
        }
    }

    public class AuditStore : EntityFrameworkAuditStore
    {
        public AuditStore(TestContext context)
            : base(context)
        {
        }
    }

    public class EventStore : EntityFrameworkEventStore, IHandleEvent
    {
        public EventStore(TestContext context)
            : base(context)
        {
        }

        public override void Append(Event instance, CommandContext context)
        {
            base.Append(instance, context);
        }
    }

    public class Program
    {
        public static void Main()
        {
            new Program().Start();

            Console.WriteLine(@"Running program.  Press any key to exit...");
            Console.WriteLine();

            Console.ReadKey();
        }

        public async Task Start()
        {
            try
            {
                var instance = new
                {
                    Name = new
                    {
                        Out = "Fred"
                    },
                    Max = 3
                };

                var content = JsonConvert.DeserializeXNode(JsonConvert.SerializeObject(instance), "Item");
                Console.WriteLine(content);


                //using (var container = new ApplicationContainer(this))
                //{
                //    container.Register<IDestructuringPolicy, LoggingDestructuringPolicy>(Guid.NewGuid().ToString());

                //    for (int i = 0; i < 2; i++)
                //    {
                //        container.Resolve<ILogger>().Error(new InvalidOperationException("xxxxx"), "ex");
                //    }

                //    Console.WriteLine(11);
                //    //var result = await container.Bus.Send(new TestCommand("content"));

                //    //Console.WriteLine(result.Successful);
                //    //Console.WriteLine(result.Elapsed);

                //    var client = new TelemetryClient
                //    {
                //        InstrumentationKey  = "a384f093-adf9-4cb9-bf75-1b7822042932"
                //    };
                //    client.Flush();
                //}
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}