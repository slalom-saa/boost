using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class ContractMappingDictionary : ConcurrentDictionary<Type, ConcurrentBag<Type>>
    {
        public IEnumerable<ContractMapping> SingleImplementationMappings
        {
            get
            {
                foreach (var item in this.Where(e => e.Key.GetAllAttributes<RuntimeBindingAttribute>().Any(x => x.BindingType == BindingType.Single)))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        public IEnumerable<ContractMapping> MultipleImplementationMappings
        {
            get
            {
                foreach (var item in
                    this.Where(e => e.Key.GetAllAttributes<RuntimeBindingAttribute>().Any(x => x.BindingType == BindingType.Multiple)))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        public IEnumerable<ContractMapping> OtherMappings
        {
            get
            {
                foreach (var item in this.Where(e => !e.Key.GetAllAttributes<RuntimeBindingAttribute>().Any()))
                {
                    foreach (var implementation in item.Value)
                    {
                        yield return new ContractMapping(item.Key, implementation);
                    }
                }
            }
        }

        public void Add(Type contract, Type implementation)
        {
            this.GetOrAdd(contract, key => new ConcurrentBag<Type>()).Add(implementation);
        }
    }
}