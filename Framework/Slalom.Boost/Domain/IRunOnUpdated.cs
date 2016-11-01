using System;
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
        /// <param name="instance">The instance that was updated.</param>
        void RunOnUpdated(params TAggregateRoot[] instance);
    }
}