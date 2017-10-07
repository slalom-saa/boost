using System;
using System.Linq;
using Slalom.Boost.Domain;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.DocumentDb
{
    /// <summary>
    /// Provides a DocumentDB based <see cref="IRepository{TRoot}"/> implementation.
    /// </summary>
    /// <typeparam name="TRoot">The type of aggregate root to use.</typeparam>
    /// <seealso cref="Slalom.Boost.Domain.IRepository{TEntity}" />
    public abstract class DocumentDbRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        [RuntimeBindingDependency]
        public DocumentDbContext Context { get; set; }

        protected DocumentDbRepository()
        {
        }

        protected DocumentDbRepository(DocumentDbContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/> instance.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/> instance.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        [RuntimeBindingDependency]
        public DocumentDbOptions Options { get; set; }

        public virtual void Add(TRoot[] instances)
        {
            this.Logger?.Verbose("Adding {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Add(instances);
        }

        public virtual void Add(TRoot[] instances, int batchSize)
        {
            this.Logger?.Verbose("Adding {Count} items of type {Type} using {Repository}.  Using a batch size of {Size}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType, batchSize);

            this.Context.Add(instances, batchSize);
        }

        public virtual void Delete()
        {
            this.Logger?.Verbose("Deleting all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Delete<TRoot>();
        }

        public virtual void Delete(TRoot[] instances)
        {
            this.Logger?.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Delete(instances);
        }

        public virtual TRoot Find(Guid id)
        {
            this.Logger?.Verbose("Finding item of type {Type} with ID {Id} using {Repository}.", typeof(TRoot).Name, id, this.GetType().BaseType);

            return this.Context.Find<TRoot>(id);
        }

        public virtual IQueryable<TRoot> Find()
        {
            this.Logger?.Verbose("Creating query for items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            return this.Context.Find<TRoot>();
        }

        public virtual void Update(TRoot[] instances)
        {
            this.Logger?.Verbose("Updating {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Context.Update(instances);
        }

        public bool Exists(Guid id)
        {
            throw new NotImplementedException();
        }

        public virtual void Update(TRoot[] instances, int batchSize)
        {
            this.Logger?.Verbose("Updating {Count} items of type {Type} using {Repository}.  Using a batch size of {Size}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType, batchSize);

            this.Context.Update(instances, batchSize);
        }
    }
}