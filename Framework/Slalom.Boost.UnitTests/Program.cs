using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Patolus.Treatment.Domain.TreatmentPlans;
using Patolus.Treatment.Persistence.Domain;
using Slalom.Boost.DocumentDb;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.MongoDB;
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
                using (var container = new ApplicationContainer(typeof(Program)))
                {
                    container.Register(new DocumentDbOptions
                    {
                        ServiceEndpoint = "https://patolus-documents.documents.azure.com:443/",
                        AuthorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==",
                        DatabaseId = "treatment",
                        CollectionId = "entries"
                    });

                    container.Register(new MongoDbOptions
                    {
                        Database = "develop",
                        UserName = "admin",
                        Password = "password",
                        Server = "ds050189.mlab.com",
                        Port = 50189
                    });

                    //container.Register(new MongoDbOptions
                    //{
                    //    Database = "treatment-development",
                    //    UserName = "service",
                    //    Password = "pass@word1",
                    //    Server = "aws-us-west-2-portal.1.dblayer.com",
                    //    Port = 15355,
                    //    UseSsl = true
                    //});


                    container.DataFacade.Add(new Item("sss"));

                    var watch = Stopwatch.StartNew();
                    for (int i = 0; i < 100; i++)
                    {
                        container.DataFacade.Find<Item>().Where(e => e.Name == "sss").Take(1).ToList().First();
                    }
                    watch.Stop();
                    Console.WriteLine(watch.Elapsed);

                    //container.Register<IRepository<TreatmentPlan>, TreatmentPlanRepository>();
                    //container.Register(new DocumentDbOptions
                    //{
                    //    ServiceEndpoint = "https://patolus-documents.documents.azure.com:443/",
                    //    AuthorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==",
                    //    DatabaseId = "treatment",
                    //    CollectionId = "entries"
                    //});


                    //var plan = container.DataFacade.Find<TreatmentPlan>().Take(1).AsEnumerable().First();

                    //var watch = Stopwatch.StartNew();
                    //plan = container.DataFacade.Find<TreatmentPlan>().Take(1).AsEnumerable().First();
                    //Console.WriteLine(watch.Elapsed);

                    ////container.Resolve<CollectionManger>().ResetDatabase();
                    ////container.Resolve<ScriptManager>().EnsureScripts();

                    ////container.DataFacade.Add(new Item("ss"));

                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}