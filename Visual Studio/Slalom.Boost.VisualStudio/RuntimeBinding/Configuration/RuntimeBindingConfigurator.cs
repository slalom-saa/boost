using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class RuntimeBindingConfigurator
    {
        private readonly List<BindingFilter> _filters;

        private List<_Assembly> _assemblies;

        public RuntimeBindingConfigurator(params BindingFilter[] filters)
        {
            _filters = this.BuildFilters(filters);
        }

        public void ConfigureContainer(IContainer container)
        {
            this.AddTypeRegistrations(container);
        }

        private List<_Assembly> LocateAndWatchAssemblyLocations(IContainer container)
        {
            var filters = _filters.OfType<AssemblyFilter>().ToArray();

            var codeBase = new CodeBaseAssemblyLocator().Locate(filters);
            var domain = new AppDomainAssemblyLocator().Locate(filters);

            var result = codeBase.Union(domain).ToList();

            domain.CollectionChanged += (a, b) =>
            {
                var resolver = new ContractMappingResolver(_filters);

                var target = resolver.Resolve(new List<_Assembly>(b.NewItems.Cast<_Assembly>()));

                AddMappings(container, target);
            };

            return result;
        }

      

        private ContractMappingDictionary ResolveContractMappings(IContainer container)
        {
            var resolver = new ContractMappingResolver(_filters);

            _assemblies = this.LocateAndWatchAssemblyLocations(container);

            var target = resolver.Resolve(_assemblies);

            return target;
        }

        private void AddTypeRegistrations(IContainer container)
        {
            var mappings = this.ResolveContractMappings(container);

            AddMappings(container, mappings);

            container.Register(container);

            foreach (var item in container.ResolveAll<IRuntimeBindingConfiguration>())
            {
                item.Configure(container);
            }
        }

        private static void AddMappings(IContainer container, ContractMappingDictionary mappings)
        {
            foreach (var mapping in mappings.SingleImplementationMappings.GroupBy(e => e.Contract))
            {
                if (mapping.Count() > 1)
                {
                    var current = mapping.Select(e => e.Implementation).Where(e => !e.GetAllAttributes<DefaultBindingAttribute>().Any()).ToList();
                    if (current.Count > 1)
                    {
                        throw new InvalidOperationException(
                            $"There are more than one types registerd for {mapping.Key} and it is specific as single only.)");
                    }
                    container.Register(mapping.Key, current.First());
                }
                else
                {
                    var target = mapping.First();
                    if (target.Implementation.GetAllAttributes<DefaultBindingAttribute>().Any(e => e.Warn))
                    {
                        Trace.TraceWarning($"The container will use the default implementation for {target.Contract} ({target.Implementation}).");
                    }
                    container.Register(mapping.Key, target.Implementation);
                }
            }

            foreach (var mapping in mappings.MultipleImplementationMappings.ToList())
            {
                var contract = mapping.Contract;
                var implementation = mapping.Implementation;
                if (implementation.GetAllAttributes<DefaultBindingAttribute>().Any(e => e.Warn))
                {
                    Trace.TraceWarning($"The container will use the default implementation for {contract} ({implementation}).");
                }
                container.Register(contract, implementation, Guid.NewGuid().ToString());
            }

            foreach (var mapping in mappings.OtherMappings.GroupBy(e => e.Contract))
            {
                if (mapping.Count() > 1)
                {
                    var current = mapping.Select(e => e.Implementation).Where(e => !e.GetAllAttributes<DefaultBindingAttribute>().Any()).ToList();
                    if (current.Count > 1 && current.Any(x => x.GetAllAttributes<DefaultBindingAttribute>().Any(y => y.Warn)))
                    {
                        Trace.TraceWarning($"The container will use the default implementation for {mapping.Key}. {current.First()} will be used.");
                    }
                    container.Register(mapping.Key, current.First());
                }
                else
                {
                    var target = mapping.First();
                    if (target.Implementation.GetAllAttributes<DefaultBindingAttribute>().Any(e => e.Warn))
                    {
                        Trace.TraceWarning($"The container will use the default implementation for {target.Contract} ({target.Implementation}).");
                    }
                    container.Register(mapping.Key, target.Implementation);
                }
            }
        }

        private List<BindingFilter> BuildFilters(BindingFilter[] filters)
        {
            var target = new List<BindingFilter>(filters);

            target.Add(AssemblyFilter.Include(e => e.FullName.StartsWith("Slalom.Boost")));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Mapping.AutoMapper") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Runtime.Humanizer") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Runtime.Dynamic") == true));

            //var entryAssembly = System.Reflection.Assembly.GetEntryAssembly();
            //if (entryAssembly != null)
            //{
            //    target.Add(AssemblyFilter.Include(e => e.FullName.StartsWith(entryAssembly.FullName.Split('.')[0])));
            //}

            foreach (var ignore in RuntimeBindingConfiguration.Ignores)
            {
                target.Add(TypeFilter.Exclude(Type.GetType(ignore.Type)));
            }

            return target;
        }
    }
}