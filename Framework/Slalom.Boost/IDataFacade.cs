using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.ReadModel;

namespace Slalom.Boost
{
    /// <summary>
    /// Provides an abstraction to persistent domain and read model objects, allows for repositories and queries to be granular and for
    /// application/infrastructure components to access objects with minimal bloat and lifetime management;  Instead of using
    /// many dependencies, in each class, for each data access component, the facade can be used and it will resolve the
    /// dependences as needed instead of on construction.
    /// </summary>
    public interface IDataFacade
    {
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
        void Add<TInstance>(TInstance instance) where TInstance : IHaveIdentity;

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
        void Add<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity;

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
        void Add<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity;

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
        void Add<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes all instances of the specified type.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance.</typeparam>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TInstance>() where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes the specified instance.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instance">The instance to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TInstance>(TInstance instance) where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes the specified instances.
        /// </summary>
        /// <typeparam name="TInstance">The type of instance to remove.</typeparam>
        /// <param name="instances">The instances to remove.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instances"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity;

        /// <summary>
        /// Removes all read models that match the specified predicate.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Delete<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate) where TReadModelElement : class, IReadModelElement;

        /// <summary>
        /// Determines if an instance with the specified identity exists.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the <c>true</c> if the instance exists; otherwise <c>false</c>.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        bool Exists<TInstance>(Guid id) where TInstance : IHaveIdentity;

        /// <summary>
        /// Finds the instance with the specified identifier.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <param name="id">The instance identifier.</param>
        /// <returns>Returns the instance with the specified identifier.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        TInstance Find<TInstance>(Guid id) where TInstance : IHaveIdentity;

        /// <summary>
        /// Finds all instances of the specified type.
        /// </summary>
        /// <typeparam name="TInstance">The type of the instance.</typeparam>
        /// <returns>An IQueryable&lt;TInstance&gt; that can be used to filter and project.</returns>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        IQueryable<TInstance> Find<TInstance>() where TInstance : IHaveIdentity;

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
        void Update<TInstance>(TInstance instance) where TInstance : IHaveIdentity;

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
        void Update<TInstance>(TInstance[] instances) where TInstance : IHaveIdentity;

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
        void Update<TInstance>(IEnumerable<TInstance> instances) where TInstance : IHaveIdentity;

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
        void Update<TInstance>(List<TInstance> instances) where TInstance : IHaveIdentity;

        /// <summary>
        /// Updates all instances found using the specified predicate and uses the provided expression for the update.
        /// </summary>
        /// <typeparam name="TReadModelElement">The type of instance.</typeparam>
        /// <param name="predicate">The predicate to match.</param>
        /// <param name="expression">The update make.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="predicate"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="expression"/> argument is null.</exception>
        /// <exception cref="System.NotSupportedException">Thrown when an unsupported type is used.</exception>
        void Update<TReadModelElement>(Expression<Func<TReadModelElement, bool>> predicate, Expression<Func<TReadModelElement, TReadModelElement>> expression) where TReadModelElement : class, IReadModelElement;
    }
}