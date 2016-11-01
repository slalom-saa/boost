using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    /// <summary>
    /// A simple <see cref="IContainer"/> implementation.
    /// </summary>
    [DefaultBinding]
    public class SimpleContainer : IContainer
    {
        private readonly IList<RegisteredObject> _registeredObjects = new List<RegisteredObject>();

        public SimpleContainer()
        {
        }

        public SimpleContainer(object instance)
        {
            this.AutoConfigure(instance.GetType());
        }

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns the resolved instance.</returns>
        public T Resolve<T>() where T : class
        {
            return this.ResolveObject(typeof(T)) as T;
        }

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns all instances of the specified type.</returns>
        public IEnumerable<T> ResolveAll<T>() where T : class
        {
            foreach (var registration in _registeredObjects.Where(e => e.Contract == typeof(T)).ToList())
            {
                yield return this.GetInstance(registration) as T;
            }
        }

        /// <summary>
        /// Resolves an instance the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns the resolved instance.</returns>
        public object Resolve(Type type)
        {
            return this.ResolveObject(type);
        }

        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        public void Register<TContract, TImplementation>() where TContract : class where TImplementation : class, TContract
        {
            _registeredObjects.Add(new RegisteredObject(typeof(TContract), typeof(TImplementation)));
        }

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        public void Register<T>(T instance) where T : class
        {
            _registeredObjects.Add(new RegisteredObject(typeof(T), instance));
        }

        /// <summary>
        /// Registers an instance of the specified type.
        /// </summary>
        /// <typeparam name="T">The contract type.</typeparam>
        /// <param name="instance">The instance to register.</param>
        /// <param name="name">The registration name.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void Register<T>(T instance, string name) where T : class
        {
            _registeredObjects.Add(new RegisteredObject(typeof(T), instance, name));
        }

        /// <summary>
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns all instances of the specified type.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            foreach (var o in _registeredObjects.Where(e => e.Contract == type))
            {
                yield return this.GetInstance(o);
            }
        }

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <typeparam name="T">The type to check.</typeparam>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        public bool CanResolve<T>()
        {
            return _registeredObjects.Any(e => e.Contract == typeof(T));
        }

        /// <summary>
        /// Determines whether the specified type can be resolved.
        /// </summary>
        /// <param name="type">The type to check.</param>
        /// <returns><c>true</c> if the specified type can be resolved; otherwise, <c>false</c>.</returns>
        public bool CanResolve(Type type)
        {
            return _registeredObjects.Any(e => e.Contract == type);
        }

        /// <summary>
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        public void Register(Type contract, Type implementation)
        {
            _registeredObjects.Add(new RegisteredObject(contract, implementation));
        }

        /// <summary>
        /// Registers an implementation of the specified type and with the specified name.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <param name="implementation">The implementation type.</param>
        /// <param name="name">The registration name.</param>
        public void Register(Type contract, Type implementation, string name)
        {
            _registeredObjects.Add(new RegisteredObject(contract, implementation));
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            foreach (var item in _registeredObjects.Where(e => e.Instance is IDisposable && e.Contract != typeof(IContainer)))
            {
                ((IDisposable)item.Instance).Dispose();
            }
        }

        /// <summary>
        /// Determines whether the specified contract is registered.
        /// </summary>
        /// <param name="contract">The contract type.</param>
        /// <returns>Returns <c>true</c> if the contract is registered; otherwise <c>false</c>.</returns>
        public bool IsRegistered(Type contract)
        {
            return _registeredObjects.Any(e => e.Contract == contract);
        }

        private object ResolveObject(Type typeToResolve)
        {
            var registeredObject = _registeredObjects.LastOrDefault(o => o.Contract == typeToResolve);
            if (registeredObject == null && typeToResolve.IsClass)
            {
                return Activator.CreateInstance(typeToResolve, this.ResolveConstructorParameters(typeToResolve).ToArray());
            }

            return this.GetInstance(registeredObject);
        }

        private void BuildUp(object item)
        {
            var types = item.GetType().GetBaseAndContractTypes().Where(e => !e.ContainsGenericParameters);
            var properties = types.SelectMany(b => b.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                   BindingFlags.Instance));

            var dependencies = properties.Where(x => x.GetCustomAttributes(typeof(RuntimeBindingDependencyAttribute)).Any());

            foreach (var property in dependencies)
            {
                property.SetValue(item, this.Resolve(property.PropertyType));
            }
        }

        private object GetInstance(RegisteredObject registeredObject)
        {
            if (registeredObject == null)
            {
                return null;
            }

            if (registeredObject.Instance == null)
            {
                var parameters = this.ResolveConstructorParameters(registeredObject);
                registeredObject.CreateInstance(parameters.ToArray());

                this.BuildUp(registeredObject.Instance);
            }

            return registeredObject.Instance;
        }

        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
        {
            return this.ResolveConstructorParameters(registeredObject.Implementation);
        }

        private IEnumerable<object> ResolveConstructorParameters(Type concrete)
        {
            var constructorInfo = concrete.GetConstructors().Where(e => e.GetParameters()
                                                                         .All(x => this.IsRegistered(x.ParameterType))).FirstOrDefault() ?? concrete.GetConstructors().First();

            if (constructorInfo != null)
            {
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    yield return this.ResolveObject(parameter.ParameterType);
                }
            }
        }

        [DebuggerDisplay("{Contract} = {Implementation}")]
        private class RegisteredObject
        {
            private string _name;

            public RegisteredObject(Type contract, Type implementation, string name = null)
            {
                this.Contract = contract;
                this.Implementation = implementation;
                _name = name;
            }

            public RegisteredObject(Type contact, object instance, string name = null)
            {
                this.Contract = contact;
                this.Instance = instance;
                _name = name;
            }

            public Type Contract { get; }

            public Type Implementation { get; }

            public object Instance { get; private set; }

            public void CreateInstance(params object[] args)
            {
                this.Instance = Activator.CreateInstance(this.Implementation, args);
            }
        }
    }
}