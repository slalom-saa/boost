using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// The class that is responsible for using locators and filters to configure the main container.
    /// </summary>
    public class RuntimeBindingConfigurator
    {
        private static readonly List<Type> DefaultImplementationWarningTypes = new List<Type>();
        private readonly List<BindingFilter> _filters;

        private List<_Assembly> _assemblies;
        private string _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingConfigurator"/> class.
        /// </summary>
        /// <param name="filters">The filters to use.</param>
        public RuntimeBindingConfigurator(params BindingFilter[] filters)
            : this(null, filters)
        {
            _filters = BuildFilters(filters);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingConfigurator" /> class.
        /// </summary>
        /// <param name="filters">The filters to use.</param>
        /// <param name="configuration">The binding configuration.</param>
        public RuntimeBindingConfigurator(string configuration, params BindingFilter[] filters)
        {
            _filters = BuildFilters(filters);
            _configuration = configuration ?? RuntimeBindingConfiguration.ConfigurationName;
        }

        /// <summary>
        /// Configures the specified container.
        /// </summary>
        /// <param name="container">The container to configure.</param>
        public void ConfigureContainer(IContainer container)
        {
            this.AddTypeRegistrations(container);
        }

        private void AddMappings(IContainer container, ContractMappingDictionary mappings)
        {
            foreach (var mapping in mappings.SingleImplementationMappings.GroupBy(e => e.Contract))
            {
                if (mapping.Count() > 1)
                {
                    var target = mapping.Select(e => e.Implementation).Where(e => !e.GetAllAttributes<DefaultBindingAttribute>().Any()).ToList();
                    if (!string.IsNullOrEmpty(_configuration))
                    {
                        var configured = target.Where(e => HasSpecificConfiguration(e, _configuration)).ToList();
                        if (configured.Any())
                        {
                            target = configured;
                        }
                        else
                        {
                            target = target.Where(e => HasDefaultConfiguration(e)).ToList();
                        }
                    }
                    else
                    {
                        target = target.Where(e => HasDefaultConfiguration(e)).ToList();
                    }
                    if (target.Count == 2)
                    {
                        var first = target.First();
                        var second = target.ElementAt(1);
                        if (first.BaseType == second)
                        {
                            target = target.Take(1).ToList();
                        }
                        else if (second.BaseType == first)
                        {
                            target = target.Skip(1).ToList();
                        }
                        else
                        {
                            throw new InvalidOperationException(string.Format(CultureInfo.CurrentCulture, "There are more than one types registered for {0} and it is specified as single only.)", mapping.Key));
                        }
                    }
                    if (target.Count > 1)
                    {
                        throw new InvalidOperationException(
                            string.Format(CultureInfo.CurrentCulture, "There are more than one types registered for {0} and it is specified as single only.)", mapping.Key));
                    }
                    if (!target.Any() && mapping.Count() == 2)
                    {
                        var first = mapping.First();
                        if (first.Implementation.BaseType == mapping.ElementAt(1).Implementation)
                        {
                            container.Register(first.Contract, first.Implementation);
                        }
                        else
                        {
                            container.Register(mapping.ElementAt(1).Contract, mapping.ElementAt(1).Implementation);
                        }
                    }
                    else
                    {
                        container.Register(mapping.Key, target.First());
                    }
                }
                else
                {
                    var target = mapping.First();

                    WarnIfDefaultImplementation(target.Implementation, target.Contract);

                    container.Register(mapping.Key, target.Implementation);
                }
            }

            foreach (var mapping in mappings.MultipleImplementationMappings.ToList())
            {
                var contract = mapping.Contract;
                var implementation = mapping.Implementation;

                WarnIfDefaultImplementation(implementation, contract);

                container.Register(contract, implementation, Guid.NewGuid().ToString());
            }

            foreach (var mapping in mappings.OtherMappings.GroupBy(e => e.Contract))
            {
                if (mapping.Count() > 1)
                {
                    var current = mapping.Select(e => e.Implementation).Where(e => !e.GetAllAttributes<DefaultBindingAttribute>().Any()).ToList();

                    WarnIfDefaultImplementation(mapping.Key, mapping.First().Implementation);

                    container.Register(mapping.Key, current.First());
                }
                else
                {
                    var target = mapping.First();

                    WarnIfDefaultImplementation(mapping.Key, target.Implementation);

                    container.Register(mapping.Key, target.Implementation);
                }
            }
        }

        private static bool HasSpecificConfiguration(Type type, string configuration)
        {
            return type.GetAllAttributes<RuntimeBindingConfigurationAttribute>().FirstOrDefault()?.Name == configuration;
        }

        private static bool HasDefaultConfiguration(Type type)
        {
            return !type.GetAllAttributes<RuntimeBindingConfigurationAttribute>().Any();
        }

        private void AddTypeRegistrations(IContainer container)
        {
            var mappings = this.ResolveContractMappings(container);

            this.AddMappings(container, mappings);

            container.Register(container);
            container.Register(new Reflection.DiscoveryService(this._assemblies.ToArray()));

            foreach (var item in container.ResolveAll<IRuntimeBindingConfiguration>())
            {
                item.Configure(container);
            }
        }

        private static List<BindingFilter> BuildFilters(BindingFilter[] filters)
        {
            var target = new List<BindingFilter>(filters);

            target.Add(AssemblyFilter.Include(e => e.FullName.StartsWith("Slalom.Boost")));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Mapping.AutoMapper") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Runtime.Humanizer") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Runtime.Dynamic") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.Search") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.EntityFramework.GraphDiff") == true));
            target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith("Slalom.Boost.EntityFramework.Extended") == true));

            foreach (var ignore in RuntimeBindingConfiguration.IgnoredTypes)
            {
                target.Add(TypeFilter.Exclude(e => e?.FullName?.StartsWith(ignore) == true));
            }

            return target;
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

                this.AddMappings(container, target);
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

        private static void WarnIfDefaultImplementation(Type implementation, Type contract)
        {
            if (implementation.GetAllAttributes<DefaultBindingAttribute>().Any(e => e.Warn))
            {
                if (!DefaultImplementationWarningTypes.Contains(contract))
                {
                    DefaultImplementationWarningTypes.Add(contract);

                    Trace.TraceWarning($"The container will use the default implementation for {contract} ({implementation}).");
                }
            }
        }
    }
}