using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using Patolus.Treatment.Domain.Notifications;
using Patolus.Treatment.Domain.Practices;
using Patolus.Treatment.Domain.Users;
using Patolus.Treatment.Persistence.Domain;
using Slalom.Boost.DocumentDb;
using Slalom.Boost.Domain;
using Slalom.Boost.RuntimeBinding;
#pragma warning disable 4014

namespace Slalom.Boost.UnitTests
{
    public class Item : Entity, IAggregateRoot
    {
        public Item(string name)
        {
            this.Name = name;
        }

        public DateTimeOffset? BirthDate { get; set; }

        public string Name { get; set; }
    }

    public class DateTimeOffsetSerializer : IBsonSerializer
    {
        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var rep = context.Reader.ReadDateTime();

            return new DateTimeOffset(rep, TimeSpan.Zero);
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
            var typed = (DateTimeOffset?)value;
            if (typed.HasValue)
            {
                context.Writer.WriteDateTime(typed.Value.UtcTicks);
            }
            else
            {
                context.Writer.WriteNull();
            }
        }

        public Type ValueType { get; }
    }

    public class ItemRepository : DocumentDbRepository<Item>
    {
    }

    public class Program
    {
        public static void Main()
        {
            new Program().Start();

            Console.WriteLine("Press any key to exit...");
            Console.WriteLine();

            Console.ReadKey();
        }

        public async Task Start()
        {
            try
            {
                using (var container = new ApplicationContainer(this))
                {
                    container.Register<IRepository<Practice>, PracticeRepository>();
                    container.Register(new DocumentDbOptions
                    {
                        ServiceEndpoint = "https://patolus-documents.documents.azure.com:443/",
                        AuthorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==",
                        DatabaseId = "treatment",
                        CollectionId = "entries"
                    });

                    container.Resolve<ScriptManager>().EnsureScripts();

                    container.DataFacade.Add(new Item("Asdf"));

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}