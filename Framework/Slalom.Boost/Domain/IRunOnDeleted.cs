using System.Threading.Tasks;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Executes some logic after a delete.
    /// </summary>
    /// <typeparam name="TAggregateRoot">The type of entity.</typeparam>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IRunOnDeleted<in TAggregateRoot> where TAggregateRoot : IAggregateRoot
    {
        /// <summary>
        /// Executes when instances of the specified type are deleted.
        /// </summary>
        /// <param name="instance">The instances that were removed.</param>
        void OnDeleted(TAggregateRoot[] instance);

        /// <summary>
        /// Executes when instances of the specified type are deleted.
        /// </summary>
        void OnDeleted();
    }
}