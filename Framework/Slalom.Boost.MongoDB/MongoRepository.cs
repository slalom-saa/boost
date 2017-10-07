using System;
using System.Linq;
using MongoDB.Bson.Serialization;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.MongoDB
{
    /// <summary>
    /// Provides a MongoDB based <see cref="IRepository{TRoot}" /> implementation.
    /// </summary>
    /// <typeparam name="TRoot">The type of aggregate root to use.</typeparam>
    /// <seealso cref="Slalom.Boost.Domain.IRepository{TEntity}" />
    public abstract class MongoRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TRoot}" /> class.
        /// </summary>
        protected MongoRepository()
        {
            MongoMappings.EnsureInitialized(this);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoRepository{TRoot}" /> class.  Use this constructor
        /// to provide a non-default connection.
        /// </summary>
        /// <param name="context">The configured <see cref="MongoDbContext" />.</param>
        protected MongoRepository(MongoDbContext context) : this()
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets configured <see cref="MongoDbContext" />.
        /// </summary>
        /// <value>The configured <see cref="MongoDbContext" />.</value>
        [RuntimeBindingDependency]
        public MongoDbContext Context { get; set; }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger" />.
        /// </summary>
        /// <value>The configured <see cref="ILogger" />.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to add.</param>
        public virtual void Add(params TRoot[] instances)
        {
            this.Logger?.Verbose("Adding {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Add(instances);
        }

        /// <summary>
        /// Removes all instances.
        /// </summary>
        public virtual void Delete()
        {
            this.Logger?.Verbose("Deleting all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Delete<TRoot>();
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        public virtual void Delete(params TRoot[] instances)
        {
            this.Logger?.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Delete(instances);
        }

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>Returns a query for all instances.</returns>
        public virtual IQueryable<TRoot> Find()
        {
            this.Logger?.Verbose("Creating query for items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            return this.Context.Find<TRoot>();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public virtual TRoot Find(Guid id)
        {
            this.Logger?.Verbose("Finding item of type {Type} with ID {Id} using {Repository}.", typeof(TRoot).Name, id, this.GetType().BaseType);

            return this.Context.Find<TRoot>(id);
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instances">The instance.</param>
        public virtual void Update(params TRoot[] instances)
        {
            this.Logger?.Verbose("Updating {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Update(instances);
        }

        public bool Exists(Guid id)
        {
            return this.Find().Any(e => e.Id == id);
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