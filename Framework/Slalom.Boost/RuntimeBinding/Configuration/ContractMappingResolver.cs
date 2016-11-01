using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Slalom.Boost.Domain;
using Slalom.Boost.Domain.Default;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Resolves contract mapping given binding filters and assemblies.
    /// </summary>
    public class ContractMappingResolver
    {
        private readonly List<BindingFilter> _filters;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContractMappingResolver"/> class.
        /// </summary>
        /// <param name="filters">The filters to use while resolving.</param>
        public ContractMappingResolver(List<BindingFilter> filters)
        {
            _filters = filters;
        }

        /// <summary>
        /// Resolves mappings for the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies to use when resolving.</param>
        /// <returns>Returns mappings for the specified assemblies.</returns>
        public ContractMappingDictionary Resolve(List<_Assembly> assemblies)
        {
            var target = new ContractMappingDictionary();

            AddConfiguredMappings(target);

            this.AddDiscoveredMappings(assemblies.GetTypes(_filters), target);

            AddDefaultGenericMappings(target, assemblies);

            return target;
        }

        private static void AddConfiguredMappings(ContractMappingDictionary target)
        {
            
        }

        private static void AddDefaultGenericMappings(ContractMappingDictionary mappings, List<_Assembly> assemblies)
        {
            Parallel.ForEach(assemblies, assembly =>
            {
                var entities =
                    assembly.SafelyGetTypes().Where(e =>
                    {
                        var bases = e.GetBaseAndContractTypes();
                        return bases.Contains(typeof(IEntity)) && bases.Contains(typeof(IAggregateRoot));
                    });

                foreach (var item in entities)
                {
                    mappings.Add(typeof(IRepository<>).MakeGenericType(item), typeof(InMemoryRepository<>).MakeGenericType(item));
                }
            });
        }

        private void AddDiscoveredMappings(IEnumerable<Type> types, ContractMappingDictionary collection)
        {
            Parallel.ForEach(types.Where(e => !e.IsAbstract && !e.IsInterface), implementor =>
            {
                var target = implementor.GetBaseAndContractTypes().Filter(_filters);
                foreach (var contract in target)
                {
                    if (IsMappingValid(contract, implementor))
                    {
                        collection.Add(contract, implementor);
                    }
                }
            });
        }

        private static bool IsMappingValid(Type contract, Type implementor)
        {
            if (implementor.IsAbstract || implementor.IsInterface)
            {
                return false;
            }

            if (!contract.IsGenericTypeDefinition)
            {
                if (!contract.IsAssignableFrom(implementor))
                {
                    return false;
                }
            }
            else if (contract.IsAbstract && (implementor.BaseType != contract))
            {
                return false;
            }

            return true;
        }
    }
}