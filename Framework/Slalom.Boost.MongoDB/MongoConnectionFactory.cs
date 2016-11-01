using System;
using System.Configuration;
using System.Linq;
using MongoDB.Driver;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// The default <see cref="IMongoConnectionFactory"/> implementation.
    /// </summary>
    /// <seealso cref="IMongoConnectionFactory" />
    [DefaultBinding]
    public class MongoConnectionFactory : IMongoConnectionFactory
    {
        /// <summary>
        /// Gets the collection with the specified database and name.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="database">The database name.</param>
        /// <param name="name">The collection name.</param>
        /// <returns>Returns the collection with the specified database and name.</returns>
        public IMongoCollection<T> GetCollection<T>(string database, string name)
        {
            return this.GetDatabase(database).GetCollection<T>(name);
        }

        /// <summary>
        /// Gets the database with the specified name.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <returns>Returns the database with the specified name.</returns>
        public IMongoDatabase GetDatabase(string name)
        {
            var client = !String.IsNullOrWhiteSpace(ConfigurationManager.AppSettings["mongo:Connection"]) ? new MongoClient(ConfigurationManager.AppSettings["mongo:Connection"])
                : new MongoClient();

            return client.GetDatabase(name ?? "local");
        }
    }
}