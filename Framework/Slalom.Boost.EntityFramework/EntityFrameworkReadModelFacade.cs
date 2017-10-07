using System;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.EntityFramework.Extensions.Extensions;
using Slalom.Boost.ReadModel;

namespace Slalom.Boost.EntityFramework
{
    /// <summary>
    /// Provides a <see href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch10.html">Read Model Façade</see> for the module.
    /// </summary>
    /// <seealso cref="IReadModelElement" />
    /// <seealso href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch10.html">Microsoft .NET: Architecting Applications for the Enterprise, Second Edition: Chapter 10. Introducing CQRS</seealso>
    public abstract class EntityFrameworkReadModelFacade : IReadModelFacade
    {
        private readonly DbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EntityFrameworkReadModelFacade"/> class.
        /// </summary>
        /// <param name="context">The current <see cref="DbContext"/> instance.</param>
        protected EntityFrameworkReadModelFacade(DbContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            _context = context;

            context.Configuration.AutoDetectChangesEnabled = false;
            context.Configuration.ValidateOnSaveEnabled = false;
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TReadModelElement">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        public virtual void Add<TReadModelElement>(params TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement
        {
            if (instances.Any())
            {
                _context.Set<TReadModelElement>().AddRange(instances);
                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The specified type.</typeparam>
        /// <returns><c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.</returns>
        public bool Contains<TEntity>()
        {
            var entityName = typeof(TEntity).Name;
            var objContext = (_context as IObjectContextAdapter)?.ObjectContext;
            var workspace = objContext?.MetadataWorkspace;
            return workspace?.GetItems<EntityType>(DataSpace.CSpace).Any(e => e.Name == entityName) ?? false;
        }

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        public virtual void Delete<TReadModelElement>() where TReadModelElement : class, IReadModelElement
        {
            _context.Set<TReadModelElement>()
                    .Delete();
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        public virtual void Delete<TReadModelElement>(TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (instances.Any())
            {
                var ids = instances.Select(e => e.Id).ToList();
                _context.Set<TReadModelElement>()
                        .Where(e => ids.Contains(e.Id))
                        .Delete();
            }
        }

        /// <summary>
        /// Removes all instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        public virtual void Delete<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate) where TReadModelElement : class, IReadModelElement
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            _context.Set<TReadModelElement>()
                    .Where(predicate)
                    .Delete();
        }

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TReadModelElement&gt; that can be used to filter and project.</returns>
        public virtual IQueryable<TReadModelElement> Find<TReadModelElement>() where TReadModelElement : class, IReadModelElement
        {
            return _context.Set<TReadModelElement>()
                           .AsNoTracking()
                           .AsQueryable();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public virtual TReadModelElement Find<TReadModelElement>(Guid id) where TReadModelElement : class, IReadModelElement
        {
            return _context.Set<TReadModelElement>()
                           .AsNoTracking()
                           .FirstOrDefault(e => e.Id == id);
        }

        public bool Exists<TReadModelElement>(Guid id) where TReadModelElement : class, IReadModelElement
        {
            return _context.Set<TReadModelElement>()
                           .Any(e => e.Id == id);
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        public virtual void Update<TReadModelElement>(TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (instances.Any())
            {
                var ids = instances.Select(e => e.Id).ToList();

                _context.Set<TReadModelElement>()
                        .Where(e => ids.Contains(e.Id))
                        .Delete();

                _context.Set<TReadModelElement>()
                        .AddRange(instances);

                _context.SaveChanges();
            }
        }

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression"/> argument is null.</exception>
        public void Update<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate, Expression<Func<TReadModelElement, TReadModelElement>> expression) where TReadModelElement : class, IReadModelElement
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            if (expression == null)
            {
                throw new ArgumentNullException(nameof(expression));
            }

            _context.Set<TReadModelElement>()
                    .Where(predicate)
                    .Update(expression);
        }
    }
}