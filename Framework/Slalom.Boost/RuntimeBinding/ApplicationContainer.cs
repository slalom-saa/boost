using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using Slalom.Boost.Aspects;
using Slalom.Boost.Domain;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// An application <see cref="IContainer"/> implementation.
    /// </summary>
    [DefaultBinding]
    public class ApplicationContainer : IContainer
    {
        private readonly List<RegisteredObject> _registeredObjects = new List<RegisteredObject>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContainer"/> class.
        /// </summary>
        public ApplicationContainer()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationContainer" /> class.
        /// </summary>
        /// <param name="instance">The instance to use to start the runtime binding.</param>
        /// <param name="configuration">The binding configuration.</param>
        public ApplicationContainer(object instance, string configuration = null)
        {
            this.AutoConfigure(instance as Type ?? instance.GetType(), configuration);
        }

        private ApplicationContainer(List<RegisteredObject> registered)
        {
            foreach (var item in registered)
            {
                this.Register(item.Contract, item.Implementation);
            }
        }

        /// <summary>
        /// Gets the message bus for the container.
        /// </summary>
        /// <value>The message bus for the container.</value>
        public IApplicationBus Bus => this.Resolve<IApplicationBus>();

        /// <summary>
        /// Gets the data facade for the container.
        /// </summary>
        /// <value>The data facade for the container.</value>
        public IDataFacade DataFacade => this.Resolve<IDataFacade>();

        /// <summary>
        /// Gets the loaded assemblies.
        /// </summary>
        /// <value>The loaded assemblies.</value>
        public IEnumerable<_Assembly> LoadedAssemblies => _registeredObjects.Select(e => e.Implementation?.Assembly).Distinct();

       

        /// <summary>
        /// Gets the loaded repositories.
        /// </summary>
        /// <value>The loaded repositories.</value>
        public IEnumerable<RegisteredObject> LoadedRepositories => _registeredObjects.Where(e => e.Contract.IsGenericTypeDefinition && e.Contract.GetGenericTypeDefinition() == typeof(IRepository<>));

        /// <summary>
        /// Gets an ordered list of the registered mappings.
        /// </summary>
        /// <value>The ordered.</value>
        public IEnumerable<RegisteredObject> Ordered => _registeredObjects.OrderBy(e => e.Contract.Name);

        /// <summary>
        /// Gets the current <see cref="IMapper"/> instance.
        /// </summary>
        /// <value>The current <see cref="IMapper"/> instance.</value>
        public IMapper Mapper => this.Resolve<IMapper>();

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

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!disposed)
                {
                    disposed = true;

                    foreach (var item in _registeredObjects.Where(e => e.Instance != null || e.CreatedInstances.Any() && e.Contract != typeof(IContainer)).SelectMany(e => e.CreatedInstances.Union(new[] { e.Instance })).Where(e => e is IDisposable))
                    {
                        ((IDisposable)item).Dispose();
                    }
                }
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
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
        /// Registers an implementation of the specified type.
        /// </summary>
        /// <typeparam name="TContract">The contract type.</typeparam>
        /// <typeparam name="TImplementation">The implementation type.</typeparam>
        /// <param name="name">The registration name.</param>
        public void Register<TContract, TImplementation>(string name) where TContract : class where TImplementation : class, TContract
        {
            _registeredObjects.Add(new RegisteredObject(typeof(TContract), typeof(TImplementation), name));
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
        /// Resolves an instance the specified type.
        /// </summary>
        /// <typeparam name="T">The type to resolve</typeparam>
        /// <returns>Returns the resolved instance.</returns>
        public T Resolve<T>() where T : class
        {
            return this.ResolveObject(typeof(T)) as T;
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
        /// Resolves all instances of the specified type.
        /// </summary>
        /// <param name="type">The type to resolve.</param>
        /// <returns>Returns all instances of the specified type.</returns>
        public IEnumerable<object> ResolveAll(Type type)
        {
            foreach (var instance in _registeredObjects.Where(e => e.Contract == type).ToList())
            {
                yield return this.GetInstance(instance);
            }
        }

        /// <summary>
        /// Creates a child container.
        /// </summary>
        /// <returns>The new <see cref="IContainer" /> instance.</returns>
        public IContainer CreateChildContainer()
        {
            return new ApplicationContainer(_registeredObjects);
        }

        /// <summary>
        /// Builds up the specified instance using the container.
        /// </summary>
        /// <param name="instance">The instance to build up.</param>
        internal void BuildUp(object instance)
        {
            if (instance != null)
            {
                var types = instance.GetType().GetBaseAndContractTypes().Where(e => !e.ContainsGenericParameters);
                if (!instance.GetType().ContainsGenericParameters)
                {
                    types = types.Concat(new[] { instance.GetType() });
                }
                var properties = types.SelectMany(b => b.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                       BindingFlags.Instance));

                var dependencies = properties.Where(x => x.GetCustomAttributes(typeof(RuntimeBindingDependencyAttribute)).Any());

                foreach (var property in dependencies)
                {
                    if (property.CanWrite && property.GetValue(instance) == null)
                    {
                        property.SetValue(instance, this.Resolve(property.PropertyType));
                    }
                }
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

        private object GetInstance(RegisteredObject registeredObject)
        {
            if (registeredObject == null)
            {
                return null;
            }

            var parameters = this.ResolveConstructorParameters(registeredObject);
            var instance = registeredObject.CreateInstance(parameters.ToArray());

            if (registeredObject.Implementation.GetAllAttributes<RuntimeBindingImplementationAttribute>().Any(e => e.BindingType == ImplementationBindingType.Singleton))
            {
                registeredObject.Instance = instance;
            }

            this.BuildUp(instance);

            return instance;
        }

        private IEnumerable<object> ResolveConstructorParameters(RegisteredObject registeredObject)
        {
            return this.ResolveConstructorParameters(registeredObject.Implementation);
        }

        private IEnumerable<object> ResolveConstructorParameters(Type concrete)
        {
            var constructorInfo = concrete.GetConstructors().Where(e => e.GetParameters()
                                                                         .All(x => this.IsRegistered(x.ParameterType))).FirstOrDefault() ?? concrete.GetConstructors().FirstOrDefault();

            if (constructorInfo != null)
            {
                foreach (var parameter in constructorInfo.GetParameters())
                {
                    yield return this.ResolveObject(parameter.ParameterType);
                }
            }
        }

        private object ResolveObject(Type typeToResolve)
        {
            if (typeToResolve.IsArray)
            {
                var target = this.ResolveAll(typeToResolve.GetElementType()).ToArray();
                var destination = Array.CreateInstance(typeToResolve.GetElementType(), target.Length);
                Array.Copy(target, destination, target.Length);
                return destination;
            }
            if (typeToResolve.IsGenericType && typeToResolve.GetBaseAndContractTypes().Any(e => e == typeof(IEnumerable)))
            {
                var target = this.ResolveAll(typeToResolve.GetGenericArguments()[0]).ToArray();
                var destination = Array.CreateInstance(typeToResolve.GetGenericArguments()[0], target.Length);
                Array.Copy(target, destination, target.Length);

                var type = typeof(List<>).MakeGenericType(typeToResolve.GetGenericArguments()[0]);
                var list = (IList)Activator.CreateInstance(type);
                foreach (var item in destination)
                {
                    list.Add(item);
                }
                return list;
            }
            var registeredObject = _registeredObjects.LastOrDefault(o => o.Contract == typeToResolve);
            if (registeredObject == null && typeToResolve.IsClass)
            {
                if (typeToResolve.GetAllAttributes<RuntimeBindingImplementationAttribute>().Any(e => e.BindingType == ImplementationBindingType.Singleton))
                {
                    var instance = Activator.CreateInstance(typeToResolve, this.ResolveConstructorParameters(typeToResolve).ToArray());
                    this.BuildUp(instance);
                    _registeredObjects.Add(new RegisteredObject(typeToResolve, instance));
                    return instance;
                }
                else
                {

                    var instance = Activator.CreateInstance(typeToResolve, this.ResolveConstructorParameters(typeToResolve).ToArray());
                    this.BuildUp(instance);
                    return instance;
                }
            }

            return registeredObject?.Instance ?? this.GetInstance(registeredObject);
        }

        /// <summary>
        /// Holds and maintains a container configured registration.
        /// </summary>
        [DebuggerDisplay("{Contract} = {Implementation}")]
        public class RegisteredObject
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="RegisteredObject" /> class.
            /// </summary>
            /// <param name="contract">The contract.</param>
            /// <param name="implementation">The implementation.</param>
            /// <param name="name">The name of the registered object.</param>
            public RegisteredObject(Type contract, Type implementation, string name = null)
            {
                this.Contract = contract;
                this.Implementation = implementation;
                this.Name = name;
            }

            /// <summary>
            /// Initializes a new instance of the <see cref="RegisteredObject"/> class.
            /// </summary>
            /// <param name="contact">The contact.</param>
            /// <param name="instance">The instance.</param>
            /// <param name="name">The name of the registered object.</param>
            public RegisteredObject(Type contact, object instance, string name = null)
            {
                this.Contract = contact;
                this.Instance = instance;
                this.Name = name;
            }

            /// <summary>
            /// Gets the contract.
            /// </summary>
            /// <value>The contract.</value>
            public Type Contract { get; }

            /// <summary>
            /// Gets the created instances.
            /// </summary>
            /// <value>The created instances.</value>
            public List<object> CreatedInstances { get; } = new List<object>();

            /// <summary>
            /// Gets the implementation.
            /// </summary>
            /// <value>The implementation.</value>
            public Type Implementation { get; }

            /// <summary>
            /// Gets the name.
            /// </summary>
            /// <value>The name.</value>
            public string Name { get; private set; }

            /// <summary>
            /// Gets or sets the instance.
            /// </summary>
            /// <value>The instance.</value>
            public object Instance { get; set; }

            /// <summary>
            /// Creates the instance.
            /// </summary>
            /// <param name="args">The arguments.</param>
            /// <returns>Returns the created instance.</returns>
            public object CreateInstance(params object[] args)
            {
                if (this.Instance != null)
                {
                    return this.Instance;
                }

                var target = Activator.CreateInstance(this.Implementation, args);

                this.CreatedInstances.Add(target);

                return target;
            }
        }
    }
}