using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.ReadModel.Default
{
    /// <summary>
    /// An in-memory <see cref="IReadModelFacade" />, intended to be used with in-process applications or for test.
    /// </summary>
    [DefaultBinding(Warn = false)]
    public class InMemoryReadModelFacade : IReadModelFacade
    {
        internal static readonly List<IReadModelElement> Items = new List<IReadModelElement>();

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TReadModel">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        public void Add<TReadModel>(TReadModel[] instances) where TReadModel : class, IReadModelElement
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            Items.AddRange(instances);
        }

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <typeparam name="T">The specified type.</typeparam>
        /// <returns><c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.</returns>
        public bool Contains<T>()
        {
            return false;
        }

        /// <summary>
        /// Removes all read models that match the specified predicate.
        /// </summary>
        /// <typeparam name="TReadModel">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        public void Delete<TReadModel>(Expression<Func<TReadModel, bool>> predicate) where TReadModel : class, IReadModelElement
        {
            if (predicate == null)
            {
                throw new ArgumentNullException(nameof(predicate));
            }

            Items.OfType<TReadModel>().AsQueryable().Where(predicate).ToList().ForEach(e =>
            {
                Items.Remove(e);
            });
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TReadModel">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        public void Delete<TReadModel>(TReadModel[] instances) where TReadModel : class, IReadModelElement
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            instances.ToList().ForEach(e =>
            {
                Items.Remove(e);
            });
        }

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModel">The type of instance.</typeparam>
        public void Delete<TReadModel>() where TReadModel : class, IReadModelElement
        {
            Items.OfType<TReadModel>().ToList().ForEach(e =>
            {
                Items.Remove(e);
            });
        }

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModel">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TReadModel&gt; that can be used to filter and project.</returns>
        public IQueryable<TReadModel> Find<TReadModel>() where TReadModel : class, IReadModelElement
        {
            return Items.OfType<TReadModel>().AsQueryable();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TReadModel">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public TReadModel Find<TReadModel>(Guid id) where TReadModel : class, IReadModelElement
        {
            return (TReadModel)Items.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TReadModel">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        public void Update<TReadModel>(TReadModel[] instances) where TReadModel : class, IReadModelElement
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }
            Items.AddRange(instances);
        }

        /// <summary>
        /// Updates all read models found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TReadModel">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <exception cref="System.NotSupportedException">Thrown when the method is called.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate" /> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression" /> argument is null.</exception>
        public void Update<TReadModel>(Expression<Func<TReadModel, bool>> predicate, Expression<Func<TReadModel, TReadModel>> expression) where TReadModel : class, IReadModelElement
        {
            throw new NotSupportedException("Not supported in an in-memory database.");
        }
    }
}