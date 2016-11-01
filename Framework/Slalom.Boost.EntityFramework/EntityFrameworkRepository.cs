using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Domain;
using Slalom.Boost.EntityFramework.GraphDiff;

namespace Slalom.Boost.EntityFramework
{
    public abstract class EntityFrameworkRepository<TEntity> : IRepository<TEntity> where TEntity : class, IAggregateRoot
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

        protected DbSet<TEntity> Set => Context.Set<TEntity>();

        public void Delete()
        {
            this.Set.RemoveRange(this.Set);
            this.Context.SaveChanges();
        }

        public void Delete(params TEntity[] instances)
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            var ids = instances.Select(e => e.Id).ToList();
            this.Set.RemoveRange(this.Set.Where(e => ids.Contains(e.Id)));
            this.Context.SaveChanges();
        }

        public virtual TEntity Find(Guid id)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            return Context.Set<TEntity>().Find(id);
        }

        public virtual IQueryable<TEntity> Find()
        {
            return Context.Set<TEntity>().AsNoTracking();
        }

        public virtual void Add(params TEntity[] instances)
        {
            this.Set.AddRange(instances);
            this.Context.SaveChanges();
        }

        public virtual void Update(params TEntity[] instances)
        {
            Context.Set<TEntity>().AddOrUpdate(instances);
            Context.SaveChanges();
        }

        protected void UpdateGraph(TEntity[] instance, Expression<Func<IUpdateConfiguration<TEntity>, object>> expression)
        {
            instance.ToList().ForEach(e =>
            {
                Context.UpdateGraph(e, expression);
            });
            Context.SaveChanges();
        }
    }
}