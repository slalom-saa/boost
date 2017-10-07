using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Domain;
using Slalom.Boost.EntityFramework.GraphDiff;
using Slalom.Boost.Logging;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.EntityFramework
{
    public abstract class EntityFrameworkRepository<TRoot> : IRepository<TRoot> where TRoot : class, IAggregateRoot
    {
        protected readonly DbContext Context;

        protected EntityFrameworkRepository(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            Context = context;
        }

        protected DbSet<TRoot> Set => Context.Set<TRoot>();

        public virtual void Delete()
        {
            this.Logger?.Verbose("Deleting all items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            this.Set.RemoveRange(this.Set);
            Context.SaveChanges();
        }

        public virtual void Delete(params TRoot[] instances)
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            this.Logger?.Verbose("Deleting {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            var ids = instances.Select(e => e.Id).ToList();
            this.Set.RemoveRange(this.Set.Where(e => ids.Contains(e.Id)));
            Context.SaveChanges();
        }

        /// <summary>
        /// Gets or sets the configured <see cref="ILogger"/> instance.
        /// </summary>
        /// <value>The configured <see cref="ILogger"/> instance.</value>
        [RuntimeBindingDependency]
        public ILogger Logger { get; set; }

        public virtual TRoot Find(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            this.Logger?.Verbose("Finding item of type {Type} with ID {Id} using {Repository}.", typeof(TRoot).Name, id, this.GetType().BaseType);

            return Context.Set<TRoot>().Find(id);
        }

        public virtual IQueryable<TRoot> Find()
        {
            this.Logger?.Verbose("Creating query for items of type {Type} using {Repository}.", typeof(TRoot).Name, this.GetType().BaseType);

            return Context.Set<TRoot>().AsNoTracking();
        }

        public virtual void Add(params TRoot[] instances)
        {
            this.Logger?.Verbose("Adding {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            this.Set.AddRange(instances);
            Context.SaveChanges();
        }

        public virtual void Update(params TRoot[] instances)
        {
            this.Logger?.Verbose("Updating {Count} items of type {Type} using {Repository}.", instances.Length, typeof(TRoot).Name, this.GetType().BaseType);

            Context.Set<TRoot>().AddOrUpdate(instances);
            Context.SaveChanges();
        }

        public bool Exists(Guid id)
        {
            return this.Find().Any(e => e.Id == id);
        }

        protected void UpdateGraph(TRoot[] instance, Expression<Func<IUpdateConfiguration<TRoot>, object>> expression)
        {
            instance.ToList().ForEach(e =>
            {
                Context.UpdateGraph(e, expression);
            });
            Context.SaveChanges();
        }
    }
}