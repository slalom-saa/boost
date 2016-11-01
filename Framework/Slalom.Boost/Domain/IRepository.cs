using System;
using System.Linq;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Defines a <see href="https://www.safaribooksonline.com/library/view/domain-driven-design-tackling/0321125215/ch06.html">Repository</see> for an <see cref="IAggregateRoot"/>.
    /// </summary>
    /// <typeparam name="TRoot">The type of <see cref="IAggregateRoot"/>.</typeparam>
    /// <seealso href="https://www.safaribooksonline.com/library/view/domain-driven-design-tackling/0321125215/ch06.html">Domain-Driven Design: Tackling Complexity in the Heart of Software: Six. The Life Cycle of a Domain Object</seealso>
    public interface IRepository<TRoot> where TRoot : IAggregateRoot
    {
        /// <summary>
        /// Removes all instances.
        /// </summary>
        void Delete();

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Delete(TRoot[] instances);

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        TRoot Find(Guid id);

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>Returns a query for all instances.</returns>
        IQueryable<TRoot> Find();

        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Add(TRoot[] instances);

        /// <summary>
        /// Updates the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        void Update(TRoot[] instances);
    }
}