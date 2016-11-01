using System;
using System.Collections.Generic;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    /// <summary>
    /// Defines a common container interface that can be used with various frameworks.
    /// </summary>
    [RuntimeBinding(BindingType.Single)]
    public interface IContainer : IDisposable
    {
        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        void Register<TContract, TImplementation>() where TContract : class where TImplementation : class, TContract;

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        void Register<T>(T instance) where T : class;

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <param name="name">The registration name.</param>
        void Register<T>(T instance, string name) where T : class;

        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        void Register(Type contract, Type implementation);

        /// <summary>
        /// Registers an implementation of the specified type and with the specified name.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        /// <param name="name">The registration name.</param>
        void Register(Type contract, Type implementation, string name);

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns the resolved instance.</returns>
        T Resolve<T>() where T : class;

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns the resolved instance.</returns>
        object Resolve(Type type);

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns all instances of the specified type.</returns>
        IEnumerable<T> ResolveAll<T>() where T : class;

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns all instances of the specified type.</returns>
        IEnumerable<object> ResolveAll(Type type);

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        bool CanResolve<T>();

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        bool CanResolve(Type type);
    }
}