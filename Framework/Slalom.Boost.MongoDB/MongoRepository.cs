using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.MongoDB.Aspects;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Provides a MongoDB based <see cref="IRepository{TRoot}"/> implementation.
    /// </summary>
    /// <typeparam name="TRoot">The type of aggregate root to use.</typeparam>
    /// <seealso cref="Slalom.Boost.Domain.IRepository{TEntity}" />
    public abstract class MongoRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TEntity}"/> class.
        /// </summary>
        protected MongoRepository()
            : this(ConfigurationManager.AppSettings["mongo:Database"])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoAuditStore" /> class.
        /// </summary>
        protected MongoRepository(string database)
        {
            this.Collection = new Lazy<IMongoCollection<TRoot>>(() => this.Factory.GetCollection<TRoot>(database, typeof(TRoot).Name));

            MongoMappings.EnsureInitialized(this);
        }

        /// <summary>
        /// Gets the collection factory.
        /// </summary>
        public Lazy<IMongoCollection<TRoot>> Collection { get; }

        /// <summary>
        /// Gets or sets the current <see cref="IMongoConnectionFactory"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMongoConnectionFactory"/> instance.</value>
        [RuntimeBindingDependency]
        public IMongoConnectionFactory Factory { get; set; }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/> instance.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/> instance.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        public virtual void Add(params TRoot[] instances)
        {
            this.Logger.Verbose("Adding {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Collection.Value.InsertMany(instances);
        }

        /// <summary>
        /// Removes all instances.
        /// </summary>
        public virtual void Delete()
        {
            this.Logger.Verbose("Deleting all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            this.Collection.Value.DeleteMany(e => true);
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        public virtual void Delete(params TRoot[] instances)
        {
            this.Logger.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            var ids = instances.Select(e => e.Id).ToList();
            this.Collection.Value.DeleteMany(e => ids.Contains(e.Id));
        }

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>Returns a query for all instances.</returns>
        public virtual IQueryable<TRoot> Find()
        {
            this.Logger.Verbose("Finding all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            return this.Collection.Value.AsQueryable();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public virtual TRoot Find(Guid id)
        {
            this.Logger.Verbose("Finding item of type {Type} with ID {Id} using {Repository}.", typeof(TRoot).Name, id, this.GetType().BaseType);

            return this.Collection.Value.Find(e => e.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instances">The instance.</param>
        public virtual void Update(params TRoot[] instances)
        {
            this.Logger.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            instances.ToList().ForEach(e =>
            {
                this.Collection.Value.ReplaceOne(x => x.Id == e.Id, e, new UpdateOptions { IsUpsert = true });
            });
        }

        /// <summary>
        /// Creates and registers class maps.
        /// </summary>
        /// <typeparam name="TClass1">The first type of class.</typeparam>
        /// <typeparam name="TClass2">The second type of class.</typeparam>
        /// <typeparam name="TClass3">The third type of class.</typeparam>
        /// <typeparam name="TClass4">The fourth type of class.</typeparam>
        public static void Register<TClass1, TClass2, TClass3, TClass4>()
        {
            Register<TClass1>();
            Register<TClass2>();
            Register<TClass3>();
            Register<TClass4>();
        }

        /// <summary>
        /// Creates and registers class maps.
        /// </summary>
        /// <typeparam name="TClass1">The first type of class.</typeparam>
        /// <typeparam name="TClass2">The second type of class.</typeparam>
        /// <typeparam name="TClass3">The third type of class.</typeparam>
        public static void Register<TClass1, TClass2, TClass3>()
        {
            Register<TClass1>();
            Register<TClass2>();
            Register<TClass3>();
        }

        /// <summary>
        /// Creates and registers class maps.
        /// </summary>
        /// <typeparam name="TClass1">The first type of class.</typeparam>
        /// <typeparam name="TClass2">The second type of class.</typeparam>
        public static void Register<TClass1, TClass2>()
        {
            Register<TClass1>();
            Register<TClass2>();
        }

        /// <summary>
        ///  Creates and registers a class map.
        /// </summary>
        /// <typeparam name="TClass">The class.</typeparam>
        public static void Register<TClass>()
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
            {
                BsonClassMap.RegisterClassMap<TClass>(e => e.AutoMap());
            }
        }

        /// <summary>
        /// Creates and registers a class map.
        /// </summary>
        /// <typeparam name="TClass">The class.</typeparam>
        /// <param name="classMapInitializer">The class map initializer.</param>
        public static void Register<TClass>(Action<BsonClassMap<TClass>> classMapInitializer)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(TClass)))
            {
                BsonClassMap.RegisterClassMap(classMapInitializer);
            }
        }
    }
}