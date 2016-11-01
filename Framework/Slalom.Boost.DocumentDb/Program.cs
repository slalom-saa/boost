using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Slalom.Boost.Domain;
using Slalom.Boost.RuntimeBinding;

#pragma warning disable 4014

namespace Slalom.Boost.DocumentDb
{
    public class Item : Entity, IAggregateRoot
    {
        public string Name { get; set; }

        public Item(string name)
        {
            this.Name = name;
        }
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

                    container.DataFacade.Add(new Item("_2"));

                    Console.WriteLine(container.DataFacade.Find<Item>().Where(e => e.Name == "_2").ToList().Count);
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception);
            }
        }
    }
}
