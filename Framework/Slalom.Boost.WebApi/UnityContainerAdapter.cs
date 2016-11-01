using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.Practices.Unity;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.WebApi
{
    public class UnityContainerAdapter : ContainerAdapter
    {
        private readonly IUnityContainer _container;

        public UnityContainerAdapter(IUnityContainer container)
        {
            _container = container;
        }

        public override bool CanResolve(Type type)
        {
            return _container.IsRegistered(type);
        }

        public override void Dispose()
        {
            _container.Dispose();
        }

        protected override void RegisterCore<T>(T instance, string name = null)
        {
            if (name == null)
            {
                _container.RegisterInstance(name, instance);
            }
            else
            {
                _container.RegisterInstance(instance);
            }
        }

        protected override void RegisterCore(Type contract, Type implementation, string name)
        {
            _container.RegisterType(contract, implementation, name);
        }

        protected override IEnumerable<object> ResolveAllCore(Type type)
        {
            foreach (var item in _container.ResolveAll(type))
            {
                this.BuildUp(item);

                yield return item;
            }
        }

        public override IContainer CreateChildContainer()
        {
            return new UnityContainerAdapter(_container.CreateChildContainer());
        }

        public override void BuildUp(object instance)
        {
            var types = instance.GetType().GetBaseAndContractTypes().Where(e => !e.ContainsGenericParameters);
            var properties = types.SelectMany(b => b.GetProperties(BindingFlags.Public | BindingFlags.NonPublic |
                                                                   BindingFlags.Instance));

            var dependencies = properties.Where(x => x.GetCustomAttributes(typeof(RuntimeBindingDependencyAttribute)).Any());

            foreach (var property in dependencies)
            {
                property.SetValue(instance, this.ResolveCore(property.PropertyType));
            }
        }

        protected override object ResolveCore(Type typeToResolve)
        {
            try
            {
                if (typeToResolve.IsArray)
                {
                    var target = this.ResolveAll(typeToResolve.GetElementType()).ToArray();
                    var destination = Array.CreateInstance(typeToResolve.GetElementType(), target.Length);
                    Array.Copy(target, destination, target.Length);
                    return destination;
                }
                else if (typeToResolve.IsGenericType && typeToResolve.GetBaseAndContractTypes().Any(e => e == typeof(IEnumerable)))
                {
                    var target = this.ResolveAll(typeToResolve.GetGenericArguments()[0]).ToArray();
                    var destination = Array.CreateInstance(typeToResolve.GetGenericArguments()[0], target.Length);
                    Array.Copy(target, destination, target.Length);

                    var type = typeof(List<>).MakeGenericType(new[] { typeToResolve.GetGenericArguments()[0] });
                    IList list = (IList)Activator.CreateInstance(type);
                    foreach (var item in destination)
                    {
                        list.Add(item);
                    }
                    return list;
                }
                else
                {
                    var item = _container.Resolve(typeToResolve);

                    this.BuildUp(item);

                    return item;
                }

            }
            catch
            {
                //Trace.TraceWarning($"A type mapping for {type} could not be found when resolving.");
                return null;
            }
        }
    }
}