using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
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

            Console.ReadLine();
        }

        public async Task Start()
        {
            try
            {



                BsonSerializer.RegisterSerializer(typeof(DateTimeOffset?), new DateTimeOffsetSerializer());

                using (var container = new ApplicationContainer(this))
                {
                    container.Register(new DocumentDbOptions
                    {
                        Host = "patolus-development.documents.azure.com",
                        Port = 10250,
                        UserName = "patolus-development",
                        Password = "Uu7Zr7w5xl92c68X6to5iBEKXcE33W1LW9l28ayjlFH5q7RdHRcQzXrkmftUWAK1Jk55Ob7ZyLAXDx7ywa4erQ==",
                        Database = "treatment",
                        Collection = "Entities"
                    });

                    var item = new Item("name")
                    {
                        BirthDate = DateTime.Now
                    };

                    container.DataFacade.Add(item);

                    var current = container.DataFacade.Find<Item>(item.Id);

                    Console.WriteLine(current?.BirthDate);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}