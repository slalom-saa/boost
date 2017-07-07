using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
    
namespace Slalom.Boost.Domain.Default
{
    /// <summary>
    /// An in-memory <see cref="IRepository{TRoot}" />, intended to be used with in-process applications or for test.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of aggregate root.</typeparam>
    /// <seealso cref="IAggregateRoot"/>
    /// <seealso cref="IRepository{TRoot}"/>
    public class InMemoryRepository<TAggregateRoot> : IRepository<TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// The in-memory instance.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        [SuppressMessage("Microsoft.Security", "CA2104:DoNotDeclareReadOnlyMutableReferenceTypes")]
        public static readonly List<TAggregateRoot> Instances = new List<TAggregateRoot>();

        [SuppressMessage("ReSharper", "StaticMemberInGenericType")]
        private static bool warned;

        /// <summary>
        /// Removes all instances.
        /// </summary>
        public void Delete()
        {
            Warn();

            Instances.Clear();
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <param name="instances">The instances to remove.</param>
        public void Delete(params TAggregateRoot[] instances)
        {
            Warn();

            instances.ToList().ForEach(e =>
            {
                Instances.Remove(e);
            });
        }

        /// <summary>
        /// Gets the entity by ID.
        /// </summary>
        /// <param name="id">The entity ID values.</param>
        /// <returns>Returns the entity with the specified ID.</returns>
        public TAggregateRoot Find(Guid id)
        {
            Warn();

            return Instances.FirstOrDefault(e => e.Id == id);
        }

        /// <summary>
        /// Finds all instances.
        /// </summary>
        /// <returns>Returns all instances.</returns>
        public IQueryable<TAggregateRoot> Find()
        {
            Warn();
            return Instances.AsQueryable();
        }

        /// <summary>
        /// Adds the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        public void Add(params TAggregateRoot[] instances)
        {
            Warn();
            Instances.AddRange(instances);
        }

        /// <summary>
        /// Updates the specified instances.
        /// </summary>
        /// <param name="instances">The instances to update.</param>
        public void Update(params TAggregateRoot[] instances)
        {
            Warn();
            Instances.AddRange(instances);
        }
        
        private static void Warn()
        {
            if (!warned)
            {
                warned = true;

                Trace.TraceWarning($"The in-memory entity store for {typeof(TAggregateRoot)} is being used.  This warning can be ignored if raised as part of a test.");
            }
        }
    }
}