using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class ContractMappingResolver
    {
        private readonly List<BindingFilter> _filters;

        public ContractMappingResolver(List<BindingFilter> filters)
        {
            _filters = filters;
        }

        public ContractMappingDictionary Resolve(List<_Assembly> assemblies)
        {
            var target = new ContractMappingDictionary();

            this.AddConfiguredMappings(target);

            this.AddDiscoveredMappings(assemblies.GetTypes(_filters), target);

            return target;
        }

        private void AddConfiguredMappings(ContractMappingDictionary target)
        {
            foreach (RuntimeBindingElement binding in RuntimeBindingConfiguration.Additions)
            {
                target.Add(Type.GetType(binding.Type), Type.GetType(binding.MapTo));
            }
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