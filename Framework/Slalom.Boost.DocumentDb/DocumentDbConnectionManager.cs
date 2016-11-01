using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using MongoDB.Driver;

namespace Slalom.Boost.DocumentDb
{
    public class DocumentDbConnectionManager
    {
        private readonly DocumentDbOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentDbConnectionManager" /> class.
        /// </summary>
        /// <param name="mappings">The mappings.</param>
        /// <param name="options">The options.</param>
        public DocumentDbConnectionManager(DocumentDbMappingsManager mappings, DocumentDbOptions options)
        {
            _options = options;

            mappings.EnsureInitialized();
        }

        /// <summary>
        /// Gets the collection with the specified database and name.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <returns>Returns the collection with the specified database and name.</returns>
        public IMongoCollection<T> GetCollection<T>()
        {
            return this.GetDatabase().GetCollection<T>(_options.Collection);
        }

        /// <summary>
        /// Gets the database with the specified name.
        /// </summary>
        /// <returns>Returns the database with the specified name.</returns>
        public IMongoDatabase GetDatabase()
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(_options.Host, _options.Port),
                UseSsl = true,
                SslSettings = new SslSettings { EnabledSslProtocols = SslProtocols.Tls12 }
            };

            var identity = new MongoInternalIdentity(_options.Database, _options.UserName);
            var evidence = new PasswordEvidence(_options.Password);

            settings.Credentials = new List<MongoCredential>
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };
            var client = new MongoClient(settings);
            return client.GetDatabase(_options.Database);
        }
    }
}