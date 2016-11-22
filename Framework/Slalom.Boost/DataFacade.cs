using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Domain;
using Slalom.Boost.ReadModel;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost
{
    /// <summary>
    /// Default implementation of the <see cref="IDataFacade"/>.
    /// </summary>
    /// <seealso cref="Slalom.Boost.IDataFacade" />
    [DefaultBinding]
    public class DataFacade : IDataFacade
    {
        private readonly IContainer _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataFacade" /> class.
        /// </summary>
        /// <param name="container">The current <see cref="IContainer"/> instance.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="container"/> argument is null.</exception>
        public DataFacade(IContainer container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }

            _container = container;
        }

        /// <summary>
        /// Adds the specified instance. Add is similar to Update, but skips a check to see if the
        /// item already exists.  
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance to add.</typeparam>
        /// <param name="instance">The instance to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Add<TInstance>(TInstance instance) where TInstance : IHaveIdentity
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            this.Add(new[] { instance });
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Add<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetReadModelFacade<TInstance>().Add((dynamic)instances);
            }
            else if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetAggregateFacade<TInstance>().Add((dynamic)instances);
            }
            else
            {
                throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be added using the data facade.");
            }
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Add<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity
        {
            this.Add(instances.ToArray());
        }

        /// <summary>
        /// Adds the specified instances. Add is similar to Update, but skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance to add.</typeparam>
        /// <param name="instances">The instances to add.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when an unsupported type is used.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public void Add<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            this.Add(instances.ToArray());
        }

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Delete<TInstance>() where TInstance : IHaveIdentity
        {
            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                var method = typeof(IReadModelFacade).GetMethods().FirstOrDefault(e => e.Name == "Delete" && e.GetParameters().Length == 0);
                method = method.MakeGenericMethod(typeof(TInstance));
                method.Invoke(this.GetReadModelFacade<TInstance>(), new object[0]);
            }
            else if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                var method = typeof(IAggregateFacade).GetMethods().FirstOrDefault(e => e.Name == "Delete" && e.GetParameters().Length == 0);
                method = method.MakeGenericMethod(typeof(TInstance));
                method.Invoke(this.GetAggregateFacade<TInstance>(), new object[0]);
            }
            else
            {
                throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be deleted using the data facade.");
            }
        }

        /// <summary>
        /// Removes the specified instance.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instance">The instance to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Delete<TInstance>(TInstance instance) where TInstance : IHaveIdentity
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.Delete(new[] { instance });
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Delete<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetReadModelFacade<TInstance>().Delete((dynamic)instances);
            }
            else if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetAggregateFacade<TInstance>().Delete((dynamic)instances);
            }
            else
            {
                throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be deleted using the data facade.");
            }
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Delete<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity
        {
            this.Delete(instances.ToArray());
        }

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when an unsupported type is used.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public void Delete<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            this.Delete(instances.ToArray());
        }

        /// <summary>
        /// Removes all read models that match the specified predicate.
        /// </summary>
        /// <typeparam name="TReadModel">The type of read model.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Delete<TReadModel>(Expression<Func<TReadModel, bool>> predicate) where TReadModel : class, IReadModelElement
        {
            this.GetReadModelFacade<TReadModel>().Delete(predicate);
        }

        /// <summary>
        /// Determines if an instance with the specified identity exists.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the <c>true</c> if the instance exists; otherwise <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public bool Exists<TInstance>(Guid id) where TInstance : IHaveIdentity
        {
            return this.Find<TInstance>().Where(e => e.Id == id).Take(1).AsEnumerable().Any();
        }

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public TInstance Find<TInstance>(Guid id) where TInstance : IHaveIdentity
        {
            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                return (TInstance)typeof(IReadModelFacade).GetMethod("Find", new[] { typeof(Guid) }).MakeGenericMethod(typeof(TInstance)).Invoke(this.GetReadModelFacade<TInstance>(), new object[] { id });
            }
            if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                return (TInstance)typeof(IAggregateFacade).GetMethod("Find", new[] { typeof(Guid) }).MakeGenericMethod(typeof(TInstance)).Invoke(this.GetAggregateFacade<TInstance>(), new object[] { id });
            }
            throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be queried using the data facade.");
        }

        /// <summary>
        /// Finds all instance(s) of the specified type.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TInstance&gt; that can be used to filter and project.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public IQueryable<TInstance> Find<TInstance>() where TInstance : IHaveIdentity
        {
            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                return (IQueryable<TInstance>)typeof(IReadModelFacade).GetMethod("Find", new Type[0]).MakeGenericMethod(typeof(TInstance)).Invoke(this.GetReadModelFacade<TInstance>(), null);
            }
            if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                return (IQueryable<TInstance>)typeof(IAggregateFacade).GetMethod("Find", new Type[0]).MakeGenericMethod(typeof(TInstance)).Invoke(this.GetAggregateFacade<TInstance>(), null);
            }
            throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be queried using the data facade.");
        }

        /// <summary>
        /// Updates the specified instance. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <param name="instance">The instance to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Update<TInstance>(TInstance instance) where TInstance : IHaveIdentity
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            this.Update(new[] { instance });
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Update<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            if (typeof(IReadModelElement).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetReadModelFacade<TInstance>().Update((dynamic)instances);
            }
            else if (typeof(IAggregateRoot).IsAssignableFrom(typeof(TInstance)))
            {
                this.GetAggregateFacade<TInstance>().Update((dynamic)instances);
            }
            else
            {
                throw new NotSupportedException($"Instances of type {typeof(TInstance).Name} cannot be updated using the data facade.");
            }
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        public void Update<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity
        {
            this.Update(instances.ToArray());
        }

        /// <summary>
        /// Updates the specified instances. Update is similar to Add, but Add skips a check to see if the
        /// item already exists.
        /// </summary>
        /// <remarks>
        /// This allows for performance gain in larger data sets.  If you are unsure
        /// and have a small set, then you can use the update method.
        /// </remarks>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <param name="instances">The instances to update.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="NotSupportedException">Thrown when an unsupported type is used.</exception>
        [SuppressMessage("Microsoft.Design", "CA1002:DoNotExposeGenericLists")]
        public void Update<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity
        {
            if (instances == null)
            {
                throw new ArgumentNullException(nameof(instances));
            }

            this.Update(instances.ToArray());
        }

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
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

            this.GetReadModelFacade<TReadModelElement>().Update(predicate, expression);
        }

        private IAggregateFacade GetAggregateFacade<TType>()
        {
            return _container.Resolve<IAggregateFacade>();
        }

        private IReadModelFacade GetReadModelFacade<TType>()
        {
            var facades = _container.ResolveAll<IReadModelFacade>();
            var target = facades.FirstOrDefault(e => e.Contains<TType>());
            if (target == null)
            {
                throw new InvalidOperationException("A read model facade that handles " + typeof(TType) + " could not be found.");
            }
            return target;
        }
    }
}