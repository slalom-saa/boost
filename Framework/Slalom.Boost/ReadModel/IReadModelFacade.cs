using System;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.ReadModel
{
    /// <summary>
    /// Provides a single access point to instances, allows for read stores to be granular and for
    /// application/infrastructure components to access objects with minimal bloat and lifetime management;  Instead of using
    /// many dependencies, in each class, for each data access component, the facade can be used and it will resolve the
    /// dependences as needed instead of on construction.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IReadModelFacade
    {
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
        void Add<TReadModelElement>(TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Determines whether this instance can handle the specified type.
        /// </summary>
        /// <typeparam name="TEntity">The specified type.</typeparam>
        /// <returns><c>true</c> if this instance can handle the specified type; otherwise, <c>false</c>.</returns>
        bool Contains<TEntity>();

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        void Delete<TReadModelElement>() where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Delete<TReadModelElement>(TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Removes all instances that match the specified predicate.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        void Delete<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate) where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TReadModelElement&gt; that can be used to filter and project.</returns>
        IQueryable<TReadModelElement> Find<TReadModelElement>() where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        TReadModelElement Find<TReadModelElement>(Guid id) where TReadModelElement : class, IReadModelElement;

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
        void Update<TReadModelElement>(TReadModelElement[] instances) where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression"/> argument is null.</exception>
        void Update<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate, Expression<Func<TReadModelElement, TReadModelElement>> expression) where TReadModelElement : class, IReadModelElement;
    }
}