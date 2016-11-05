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
    [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
    public class MongoDbContext
    {
        public MongoDbContext()
        {
        }

        public MongoDbContext(MongoDbOptions options)
        {
            this.Options = options;
        }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/> instance.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/> instance.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Gets or sets the configured <see cref="MongoDbOptions"/> instance.
        /// </summary>
        /// <value>The configured <see cref="MongoDbOptions"/> instance.</value>
        [RuntimeBindingDependency]
        public MongoDbOptions Options { get; set; }

        public virtual void Add<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            this.GetCollection<TRoot>().InsertMany(instances);
        }

        public virtual void Delete<TRoot>() where TRoot : IAggregateRoot
        {
            this.GetCollection<TRoot>().DeleteMany(e => true);
        }

        public virtual void Delete<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            var ids = instances.Select(e => e.Id).ToList();
            this.GetCollection<TRoot>().DeleteMany(e => ids.Contains(e.Id));
        }

        public virtual IQueryable<TRoot> Find<TRoot>() where TRoot : IAggregateRoot
        {
            return this.GetCollection<TRoot>().AsQueryable();
        }

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
            name = name ?? typeof(T).Name;

            return this.GetDatabase(this.Options.Database).GetCollection<T>(name);
        }

        /// <summary>
        /// Gets the database with the specified name.
        /// </summary>
        /// <param name="name">The database name.</param>
        /// <returns>Returns the database with the specified name.</returns>
        public IMongoDatabase GetDatabase(string name)
        {
            var settings = new MongoClientSettings
            {
                Server = new MongoServerAddress(this.Options.Server, this.Options.Port),

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

            settings.Credentials = new List<MongoCredential>()
            {
                new MongoCredential("SCRAM-SHA-1", identity, evidence)
            };

            var client = new MongoClient(settings);
            return client.GetDatabase(this.Options.Database);
        }

        public virtual void Update<TRoot>(TRoot[] instances) where TRoot : IAggregateRoot
        {
            instances.ToList().ForEach(e =>
            {
                this.GetCollection<TRoot>().ReplaceOne(x => x.Id == e.Id, e, new UpdateOptions { IsUpsert = true });
            });
        }
    }
}