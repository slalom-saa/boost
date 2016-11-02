using System;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Slalom.Boost.Domain;

namespace Slalom.Boost.DocumentDb
{
    /// <summary>
    /// Provides a DocumentDB based <see cref="IRepository{TRoot}"/> implementation.
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
            this.Collection = new Lazy<IMongoCollection<DocumentItem<TRoot>>>(() => this.Factory.GetCollection<DocumentItem<TRoot>>());
        }

        /// <summary>
        /// Gets the collection factory.
        /// </summary>
        public Lazy<IMongoCollection<DocumentItem<TRoot>>> Collection { get; }

        /// <summary>
        /// Gets or sets the configured <see cref="DocumentDbConnectionManager"/> instance.
        /// </summary>
        /// <value>The current <see cref="DocumentDbConnectionManager"/> instance.</value>
        [RuntimeBinding.RuntimeBindingDependency]
        public DocumentDbConnectionManager Factory { get; set; }

        public virtual void Delete()
        {
            this.Collection.Value.DeleteMany(e => true);
        }

        public virtual void Delete(TRoot[] instances)
        {
            var ids = instances.Select(e => e.Id).ToList();
            this.Collection.Value.DeleteMany(e => ids.Contains(e.Id));
        }

        public virtual TRoot Find(Guid id)
        {
            var target = this.Collection.Value.Find(e => e.Id == id).FirstOrDefault();

            return target.Value;
        }

        public virtual IQueryable<TRoot> Find()
        {
            return this.Collection.Value.AsQueryable().Where(e => e.PartitionKey == typeof(TRoot).Name).Select(e => e.Value);
        }

        public virtual void Add(TRoot[] instances)
        {
            this.Collection.Value.InsertMany(instances.Select(e => new DocumentItem<TRoot>(e)));
        }

        public virtual void Update(TRoot[] instances)
        {
            instances.ToList().ForEach(e =>
            {
                this.Collection.Value.ReplaceOne(x => x.Id == e.Id, new DocumentItem<TRoot>(e), new UpdateOptions { IsUpsert = true });
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

        public virtual Task RemoveAsync(TRoot[] instances)
        {
            var ids = instances.Select(e => e.Id).ToList();
            return this.Collection.Value.DeleteManyAsync(e => ids.Contains(e.Id));
        }

        public virtual Task UpdateAsync(TRoot[] instances)
        {
            return Task.WhenAll(instances.ToList().Select(e =>
            {
                return this.Collection.Value.ReplaceOneAsync(x => x.Id == e.Id, new DocumentItem<TRoot>(e), new UpdateOptions { IsUpsert = true });
            }));
        }
    }
}