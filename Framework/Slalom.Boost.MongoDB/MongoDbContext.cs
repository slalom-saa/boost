using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Authentication;
using MongoDB.Driver;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// A context for accessing items in MongoDB.
    /// </summary>
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class MongoDbContext
    {
        private IMongoDatabase _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbContext"/> class.
        /// </summary>
        public MongoDbContext()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoDbContext"/> class.  Use this constructor
        /// to use a non-default connection.
        /// </summary>
        /// <param name="options">The configured <see cref="MongoDbOptions"/>.</param>
        public MongoDbContext(MongoDbOptions options) : this()
        {
            this.Options = options;
        }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/>.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/>.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the configured <see cref="MongoDbOptions"/>.
        /// </summary>
        /// <value>The configured <see cref="MongoDbOptions"/>.</value>
        [RuntimeBindingDependency]
        public MongoDbOptions Options { get; set; }

        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to add.</param>
        public virtual void Add<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            this.GetCollection<TRoot>().InsertMany(instances);
        }

        /// <summary>
        /// Removes all instances.
        /// </summary>
        public virtual void Delete<TRoot>() where TRoot : IAggregateRoot
        {
            this.GetCollection<TRoot>().DeleteMany(e => true);
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        public virtual void Delete<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            var ids = instances.Select(e => e.Id).ToList();
            this.GetCollection<TRoot>().DeleteMany(e => ids.Contains(e.Id));
        }

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>Returns a query for all instances.</returns>
        public virtual IQueryable<TRoot> Find<TRoot>() where TRoot : IAggregateRoot
        {
            return this.GetCollection<TRoot>().AsQueryable();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public virtual TRoot Find<TRoot>(Guid id) where TRoot : IAggregateRoot
        {
            return this.GetCollection<TRoot>().Find(e => e.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Gets the collection with the specified name.
        /// </summary>
        /// <typeparam name="T">The type of collection.</typeparam>
        /// <param name="name">The collection name.</param>
        /// <returns>Returns the collection with the specified name.</returns>
        public IMongoCollection<T> GetCollection<T>(string name = null)
        {
            name = name ?? typeof(T).Name.Pluralize();

            return this.GetDatabase().GetCollection<T>(name);
        }

        /// <summary>
        /// Gets the configured database.
        /// </summary>
        /// <returns>Returns the configured database.</returns>
        public IMongoDatabase GetDatabase()
        {
            if (_database == null)
            {
                if (string.IsNullOrWhiteSpace(this.Options.Server))
                {
                    this.Logger?.Verbose("Initializing local MongoDB connection.");

                    var client = new MongoClient("mongodb://localhost:27017/?3t.connection.name=local+-+imported+on+May+19%2C+2016&3t.uriVersion=2&3t.connectionMode=direct&readPreference=primary");
                    _database = client.GetDatabase(this.Options.Database ?? "local");
                }
                else
                {
                    this.Logger?.Verbose("Initializing MongoDB connection with {Server} {Port} {UserName} {Database}.", this.Options.Server, this.Options.Port, this.Options.UserName, this.Options.Database);

                    var settings = new MongoClientSettings
                    {
                        Server = new MongoServerAddress(this.Options.Server, this.Options.Port)
                    };
                    if (this.Options.UseSsl)
                    {
                        settings.UseSsl = true;
                        settings.SslSettings = new SslSettings
                        {
                            EnabledSslProtocols = SslProtocols.Tls12,
                            ServerCertificateValidationCallback = (a, b, c, d) => true
                        };
                    }

                    var identity = new MongoInternalIdentity(this.Options.Database, this.Options.UserName);
                    var evidence = new PasswordEvidence(this.Options.Password);

                    settings.Credentials = new List<MongoCredential>
                    {
                        new MongoCredential("SCRAM-SHA-1", identity, evidence)
                    };

                    var client = new MongoClient(settings);
                    _database = client.GetDatabase(this.Options.Database);
                }
            }
            return _database;
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instances">The instance.</param>
        public virtual void Update<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            instances.ToList().ForEach(e =>
            {
                this.GetCollection<TRoot>().ReplaceOne(x => x.Id == e.Id, e, new UpdateOptions { IsUpsert = true });
            });
        }
    }
}