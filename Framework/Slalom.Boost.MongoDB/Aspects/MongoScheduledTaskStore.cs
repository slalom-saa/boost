using System;
using System.Configuration;
using System.Linq;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.Tasks;

namespace Slalom.Boost.MongoDB.Aspects
{
    /// <summary>
    /// Provides a MongoDB based <see cref="IScheduledTaskStore"/> implementation.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Tasks.IScheduledTaskStore" />
    public abstract class MongoScheduledTaskStore : IScheduledTaskStore
    {
        static MongoScheduledTaskStore()
        {
            BsonClassMap.RegisterClassMap<ScheduledTask>(e =>
            {
                e.AutoMap();
                e.MapField("_commandType");
            });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoScheduledTaskStore"/> class.
        /// </summary>
        protected MongoScheduledTaskStore()
            : this(ConfigurationManager.AppSettings["mongo:Database"])
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MongoScheduledTaskStore"/> class.
        /// </summary>
        /// <param name="database">The database.</param>
        protected MongoScheduledTaskStore(string database)
        {
            this.Collection = new Lazy<IMongoCollection<ScheduledTask>>(() => (this.Factory ?? new MongoConnectionFactory()).GetCollection<ScheduledTask>(database, "ScheduledTasks"));

            MongoMappings.EnsureInitialized(this);
        }

        /// <summary>
        /// Gets the collection factory.
        /// </summary>
        public Lazy<IMongoCollection<ScheduledTask>> Collection { get; }

        /// <summary>
        /// Gets or sets the current <see cref="IMongoConnectionFactory"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMongoConnectionFactory"/> instance.</value>
        [RuntimeBindingDependency]
        public IMongoConnectionFactory Factory { get; set; }

        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Add(ScheduledTask instance)
        {
            if (this.Find().Any(x => x.Name == instance.Name))
            {
                throw new InvalidOperationException("A task cannot be added with the name of an existing command.");
            }

            this.Collection.Value.InsertOne(instance);
        }

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Update(ScheduledTask instance)
        {
            this.Collection.Value.ReplaceOne(x => x.Id == instance.Id, instance, new UpdateOptions { IsUpsert = true });
        }

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        public void Delete(ScheduledTask instance)
        {
            this.Collection.Value.DeleteOne(e => e.Id == instance.Id);
        }

        /// <summary>
        /// Deletes all scheduled task instances.
        /// </summary>
        public void Delete()
        {
            this.Collection.Value.DeleteMany(e => true);
        }

        /// <summary>
        /// Finds scheduled tasks.
        /// </summary>
        /// <returns>A query to access scheduled tasks.</returns>
        public IQueryable<ScheduledTask> Find()
        {
            return this.Collection.Value.AsQueryable();
        }
    }
}