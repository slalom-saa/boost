using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Provides a concurrent dictionary for contract mapping.
    /// </summary>
    public class ContractMappingDictionary : ConcurrentDictionary<Type, ConcurrentBag<Type>>
    {
        /// <summary>
        /// Gets contract mappings where there are multiple implementations intended.
        /// </summary>
        /// <value>The contract mappings where there are multiple implementations intended.</value>
        public IEnumerable<ContractMapping> MultipleImplementationMappings
        {
            get
            {
                foreach (var item in
                    this.Where(e => e.Key.GetAllAttributes<RuntimeBindingContractAttribute>().Any(x => x.ContractBindingType == ContractBindingType.Multiple)))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        /// <summary>
        /// Gets mappings that do not have single or multiple indicated.
        /// </summary>
        /// <value>The mappings that do not have single or multiple indicated.</value>
        public IEnumerable<ContractMapping> OtherMappings
        {
            get
            {
                foreach (var item in this.Where(e => !e.Key.GetAllAttributes<RuntimeBindingContractAttribute>().Any()))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        /// <summary>
        /// Gets contract mappings where there is a single implementation intended.
        /// </summary>
        /// <value>The contract mappings where there is a single implementation intended.</value>
        public IEnumerable<ContractMapping> SingleImplementationMappings
        {
            get
            {
                foreach (var item in this.Where(e => e.Key.GetAllAttributes<RuntimeBindingContractAttribute>().Any(x => x.ContractBindingType == ContractBindingType.Single)))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        /// <summary>
        /// Adds the specified mapping to the dictionary.
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="implementation">The implementation.</param>
        public void Add(Type contract, Type implementation)
        {
            this.GetOrAdd(contract, key => new ConcurrentBag<Type>()).Add(implementation);
        }
    }
}