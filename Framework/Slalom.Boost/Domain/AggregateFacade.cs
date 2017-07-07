using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using Slalom.Boost.Configuration;

namespace Slalom.Boost.Domain
{
    /// <summary>Default implementation for the <see cref="IAggregateFacade"/>.</summary>
    /// <seealso cref="IAggregateFacade" />
    public class AggregateFacade : IAggregateFacade
    {
        private readonly IComponentContext _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateFacade"/> class.
        /// </summary>
        /// <param name="container">The container to use.</param>
        public AggregateFacade(IComponentContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = container;
        }

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
        public void Add<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }

            var repository = _container.Resolve<IRepository<TAggregateRoot>>();
            repository.Add(instances);

            _container.ResolveAll<IRunOnUpdated<TAggregateRoot>>().ToList().ForEach(e => e.RunOnUpdated(instances));
        }

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of instance.</typeparam>
        public void Delete<TAggregateRoot>() where TAggregateRoot : IAggregateRoot
        {
            _container.Resolve<IRepository<TAggregateRoot>>().Delete();

            _container.ResolveAll<IRunOnDeleted<TAggregateRoot>>().ToList().ForEach(e => e.OnDeleted());
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        public void Delete<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }

            _container.Resolve<IRepository<TAggregateRoot>>().Delete(instances);

            _container.ResolveAll<IRunOnDeleted<TAggregateRoot>>().ToList().ForEach(e => e.OnDeleted(instances));
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        public TAggregateRoot Find<TAggregateRoot>(Guid id) where TAggregateRoot : IAggregateRoot
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }
            return _container.Resolve<IRepository<TAggregateRoot>>().Find(id);
        }

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TAggregateRoot">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TAggregateRoot&gt; that can be used to filter and project.</returns>
        public IQueryable<TAggregateRoot> Find<TAggregateRoot>() where TAggregateRoot : IAggregateRoot
        {
            return _container.Resolve<IRepository<TAggregateRoot>>().Find();
        }

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
        public void Update<TAggregateRoot>(TAggregateRoot[] instances) where TAggregateRoot : IAggregateRoot
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (!instances.Any())
            {
                return;
            }
            var instancesIds = instances.Select(i => i.Id);
            var updatedInstances = this.Find<TAggregateRoot>().Select(i => i.Id).Where(ui => instancesIds.Contains(ui)).ToList();

            _container.Resolve<IRepository<TAggregateRoot>>().Update(instances);

            _container.ResolveAll<IRunOnUpdated<TAggregateRoot>>().ToList().ForEach(e => e.RunOnUpdated(instances, updatedInstances));
        }
    }
}