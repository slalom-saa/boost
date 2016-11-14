using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using Serilog.Core;
using Slalom.Boost.Commands;
using Slalom.Boost.EntityFramework;
using Slalom.Boost.Events;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;
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
        [Ignore]
        public string Content { get; }

        public TestCommand(string content)
        {
            this.Content = content;
        }
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

    public class AuditStore : EntityFramework.Aspects.EntityFrameworkAuditStore
    {
        public AuditStore(TestContext context)
            : base(context)
        {
        }
    }

    public class EventStore : EntityFramework.Aspects.EntityFrameworkEventStore, IHandleEvent
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
                using (var container = new ApplicationContainer(this))
                {
                    container.Register<IDestructuringPolicy, LoggingDestructuringPolicy>(Guid.NewGuid().ToString());


                    var result = await container.Bus.Send(new TestCommand("content"));

                    Console.WriteLine(result.Successful);
                    Console.WriteLine(result.Elapsed);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}