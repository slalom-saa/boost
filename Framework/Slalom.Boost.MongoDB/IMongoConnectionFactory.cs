using MongoDB.Driver;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Defines a contract for MongoDB connection factories.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IMongoConnectionFactory
    {
        /// <summary>
        /// Gets the collection with the specified database and name.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="database">The database name.</param>
        /// <param name="name">The collection name.</param>
        /// <returns>Returns the collection with the specified database and name.</returns>
        IMongoCollection<T> GetCollection<T>(string database, string name);

        /// <summary>
        /// Gets the database with the specified name.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <returns>Returns the database with the specified name.</returns>
        IMongoDatabase GetDatabase(string name);
    }
}