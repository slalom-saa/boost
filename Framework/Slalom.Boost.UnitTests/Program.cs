using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using Newtonsoft.Json;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Domain;
using Slalom.Boost.EntityFramework;
using Slalom.Boost.Events;
using Slalom.Boost.MongoDB;
using Slalom.Boost.MongoDB.Aspects;
using Slalom.Boost.ReadModel;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.Tasks;

namespace Slalom.Boost.UnitTests
{

    public class AddMemberCommand : Command<MemberAddedEvent>
    {
        public string Name { get; private set; }

        public AddMemberCommand(string name)
        {
            this.Name = name;
        }
    }

    public class AddMemberCommandHandler : CommandHandler<AddMemberCommand, MemberAddedEvent>
    {
        public override MemberAddedEvent HandleCommand(AddMemberCommand command)
        {
            Console.WriteLine(command.Name);

            return new MemberAddedEvent();
        }
    }

    [Serializable]
    public class MemberAddedEvent : Event
    {
    }

    public class ScheduledTaskRepository : MongoScheduledTaskStore
    {

    }

    public class TestContext : BoostDbContext
    {
        public DbSet<TestReadModel> Items { get; set; }
    }

    public class Facade : EntityFrameworkReadModelFacade
    {
        public Facade(TestContext context) : base(context)
        {
        }
    }

    public class TestReadModel : IReadModelElement
    {
        public Guid Id { get; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                using (var container = new ApplicationContainer(typeof(Program)))
                {

                    container.DataFacade.Add(new TestReadModel());


                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}