using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Threading.Tasks;
using System.Xml.Linq;
using Microsoft.ApplicationInsights;
using Newtonsoft.Json;
using Serilog.Core;
using Slalom.Boost.Commands;
using Slalom.Boost.EntityFramework;
using Slalom.Boost.EntityFramework.Logging;
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
            //throw new Exception("...");
            return new TestEvent(command.Content);
        }
    }

    public class TestContext : BoostDbContext
    {
        public TestContext() : base("Name=Local")
        {
        }
    }

    public class AuditStore : SqlAuditStore
    {
        public AuditStore(LoggingDbContext context)
            : base(context)
        {
        }
    }

    public class EventStore : SqlEventStore
    {
        public EventStore(LoggingDbContext context)
            : base(context)
        {
        }
    }

    public class Program
    {
        public static void Main()
        {
            try
            {
                using (var container = new ApplicationContainer(typeof(Program)))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        container.Bus.Send(new TestCommand("content")).Wait();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }

        }

        public async Task Start()
        {
            try
            {
                using (var container = new ApplicationContainer(this))
                {
                    for (int i = 0; i < 2; i++)
                    {
                        container.Bus.Send(new TestCommand("content")).Wait();
                    }
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}