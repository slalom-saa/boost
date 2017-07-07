using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Executes some logic after an update.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of entity.</typeparam>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IRunOnUpdated<in TAggregateRoot>
    {
        /// <summary>
        /// Executes logic when the specified instance is updated.
        /// </summary>
        /// <param name="instance">The instance that was updated or added.</param>
        /// <param name="updatedInstances">The instances that were already present before update operation was performed. These are objects that are guaranteed to be updated NOT added.</param>
        void RunOnUpdated(TAggregateRoot[] instance, IEnumerable<Guid> updatedInstances = null);
    }
}