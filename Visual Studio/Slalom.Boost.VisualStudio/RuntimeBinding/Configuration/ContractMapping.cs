using System;
using System.Diagnostics;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    [DebuggerDisplay("{Contract} - {Implementation}")]
    public class ContractMapping
    {
        public ContractMapping(Type contract, Type implementation)
        {
            this.Contract = contract;
            this.Implementation = implementation;
        }

        public Type Contract { get; private set; }

        public Type Implementation { get; private set; }
    }
}