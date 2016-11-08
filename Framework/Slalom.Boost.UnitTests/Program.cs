using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Authentication;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
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
using ConnectionMode = MongoDB.Driver.ConnectionMode;

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
                    container.Register<IRepository<TreatmentPlan>, TreatmentPlanRepository>();

                    var plan = TreatmentPlan.Create("asdf", Guid.Empty, Guid.Empty);
                    plan.AddProcedure(Procedure.Create("asdf", "asdf", Guid.Empty, 1, "adf", "asdf", 2, 2, "asdf"));

                    container.DataFacade.Add(plan);

                    return;

                    container.Register(new DocumentDbOptions
                    {
                        ServiceEndpoint = "https://patolus-documents.documents.azure.com:443/",
                        AuthorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==",
                        DatabaseId = "treatment",
                        CollectionId = "entries"
                    });

                    container.Register(new MongoDbOptions
                    {
                        Database = "treatment-dev",
                        UserName = "service",
                        Password = "pass@word1",
                        Server = "aws-us-west-2-portal.1.dblayer.com",
                        Port = 15370,
                        UseSsl = true
                    });


                    //RunDocumentDbTest();
                    RunMongoTest();


                    //container.DataFacade.Delete<Item>();

                    //var target = new List<Item>();
                    //for (int i = 0; i < 2000; i++)
                    //{
                    //    target.Add(new Item("Item " + i));
                    //}

                    //container.DataFacade.Add(target);

                    //var watch = Stopwatch.StartNew();
                    //for (int i = 0; i < 100; i++)
                    //{
                    //    var item = container.DataFacade.Find<Item>().Take(1).AsEnumerable().First();
                    //    if (i % 10 == 0)
                    //    {
                    //        Console.WriteLine(item + " " + i);
                    //    }
                    //}
                    //Console.WriteLine(watch.Elapsed);

                   
                    return;


                    //for (int i = 0; i < 100; i++)
                    //{
                    //    var item = client.CreateDocumentQuery<DocumentItem<Item>>(collectionUri)
                    //                     .Where(e => e.PartitionKey == typeof(Item).Name)
                    //                     .Select(e => e.Value).Take(1).AsEnumerable().First() + " " + i;
                    //    if (i % 10 == 0)
                    //    {
                    //        Console.WriteLine(item);
                    //    }
                    //}


                    //container.Register(new MongoDbOptions
                    //{
                    //    Database = "develop",
                    //    UserName = "admin",
                    //    Password = "password",
                    //    Server = "ds050189.mlab.com",
                    //    Port = 50189
                    //});

                    //container.Register(new MongoDbOptions
                    //{
                    //    Database = "treatment-development",
                    //    UserName = "service",
                    //    Password = "pass@word1",
                    //    Server = "aws-us-west-2-portal.1.dblayer.com",
                    //    Port = 15355,
                    //    UseSsl = true
                    //});

                    RunMongoTest();
                    RunDocumentDbTest();

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

        private static void RunMongoTest()
        {
            var options = new MongoDbOptions
            {
                Database = "treatment-dev",
                UserName = "service",
                Password = "pass@word1",
                Server = "aws-us-west-2-portal.1.dblayer.com",
                Port = 15370,
                UseSsl = true
            };
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(options.Server, options.Port),

            };

            var identity = new MongoInternalIdentity(options.Database, options.UserName);
            var evidence = new PasswordEvidence(options.Password);

            settings.Credentials = new List<MongoCredential>
                    {
                        new MongoCredential("SCRAM-SHA-1", identity, evidence)
                    };

            var client = new MongoClient(settings);
            var database = client.GetDatabase(options.Database);

            var collection = database.GetCollection<Item>("Items");

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var item = collection.AsQueryable().Take(1).AsEnumerable().First();
                if (i % 10 == 0)
                {
                    Console.WriteLine("MongoDB " + i);
                }
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }

        private static void RunDocumentDbTest()
        {
            var options = new DocumentDbOptions
            {
                ServiceEndpoint = "https://patolus-documents.documents.azure.com:443/",
                AuthorizationKey = "ASdxo55GhyAEXKFCyK21kkQpsu09XTmb6mYmrJhxe0hyllq7b7jfCuhSeZ6JrmPIQvfAcQxWtL8IJkLJIjY4Qw==",
                DatabaseId = "treatment",
                CollectionId = "entries"
            };
            var client = new DocumentClient(new Uri(options.ServiceEndpoint), options.AuthorizationKey, new ConnectionPolicy
            {
                ConnectionMode = Microsoft.Azure.Documents.Client.ConnectionMode.Direct,
                ConnectionProtocol = Protocol.Tcp
            });
            client.OpenAsync().Wait();
            var collectionUri = UriFactory.CreateDocumentCollectionUri(options.DatabaseId, options.CollectionId);

            var watch = Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
            {
                var item = client.CreateDocumentQuery<DocumentItem<Item>>(collectionUri)
                                 .Where(e => e.PartitionKey == typeof(Item).Name)
                                 .Select(e => e.Value).Take(1).AsEnumerable().First();
                if (i % 10 == 0)
                {
                    Console.WriteLine("DocumentDB " + i);
                }
            }
            watch.Stop();
            Console.WriteLine(watch.Elapsed);
        }
    }
}