using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Provides a base class for container adapters.
    /// </summary>
    public abstract class ContainerAdapter : IContainer
    {
        /// <summary>
        /// Gets the message bus for the container.
        /// </summary>
        /// <value>The message bus for the container.</value>
        public IApplicationBus Bus => this.Resolve<IApplicationBus>();

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        public bool CanResolve<T>()
        {
            return this.CanResolve(typeof(T));
        }

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        public abstract bool CanResolve(Type type);

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        [SuppressMessage("Microsoft.Design", "CA1063:ImplementIDisposableCorrectly")]
        public abstract void Dispose();

        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        public void Register<TContract, TImplementation>() where TContract : class where TImplementation : class, TContract
        {
            this.Register(typeof(TContract), typeof(TImplementation));
        }

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        public void Register<T>(T instance) where T : class
        {
            this.RegisterCore(instance);
        }

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <param name="name">The registration name.</param>
        public void Register<T>(T instance, string name) where T : class
        {
            this.RegisterCore(instance, name);
        }

        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        public void Register(Type contract, Type implementation)
        {
            this.RegisterCore(contract, implementation, null);
        }

        /// <summary>
        /// Registers an implementation of the specified type and with the specified name.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        /// <param name="name">The registration name.</param>
        public void Register(Type contract, Type implementation, string name)
        {
            this.RegisterCore(contract, implementation, name);
        }

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns the resolved instance.</returns>
        public T Resolve<T>() where T : class
        {
            return this.Resolve(typeof(T)) as T;
        }

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns the resolved instance.</returns>
        public object Resolve(Type type)
        {
            return this.ResolveCore(type);
        }

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns all instances of the specified type.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            return this.ResolveAll(typeof(T)).Select(e => e as T);
        }

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns all instances of the specified type.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            return this.ResolveAllCore(type);
        }

        /// <summary>
        /// Creates a child container.
        /// </summary>
        /// <returns>The new <see cref="IContainer" /> instance.</returns>
        public abstract IContainer CreateChildContainer();

        /// <summary>
        /// Builds up the specified instance using the container.
        /// </summary>
        /// <param name="instance">The instance to build up.</param>
        public abstract void BuildUp(object instance);

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <param name="name">The name of the instance.</param>
        protected abstract void RegisterCore<T>(T instance, string name = null) where T : class;

        /// <summary>
        /// Registers an implementation of the specified type and with the specified name.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        /// <param name="name">The registration name.</param>
        protected abstract void RegisterCore(Type contract, Type implementation, string name);

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns all instances of the specified type.</returns>
        protected abstract IEnumerable<object> ResolveAllCore(Type type);

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns the resolved instance.</returns>
        protected abstract object ResolveCore(Type type);
    }
}