using System;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Events;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Provides a single access point to aggregates, allows for repositories to be granular and for
    /// application/infrastructure components to access objects with minimal bloat and lifetime management;  Instead of using
    /// many dependencies, in each class, for each data access component, the facade can be used and it will resolve the
    /// dependences as needed instead of on construction.
    /// </summary>
    public interface IAggregateFacade
    {
        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TAggregateRoot">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Add<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of instance.</typeparam>
        void Delete<TAggregateRoot>() where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Delete<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        TAggregateRoot Find<TAggregateRoot>(Guid id) where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TAggregateRoot&gt; that can be used to filter and project.</returns>
        IQueryable<TAggregateRoot> Find<TAggregateRoot>() where TAggregateRoot : IAggregateRoot;

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TAggregateRoot">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Update<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot;
    }
}