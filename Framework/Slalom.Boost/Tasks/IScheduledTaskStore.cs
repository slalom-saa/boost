using System;
using System.Linq;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Tasks
{
    /// <summary>
    /// Defines a contract for storing scheduled tasks and retrieving them.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IScheduledTaskStore
    {
        /// <summary>
        /// Adds the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Add(ScheduledTask instance);

        /// <summary>
        /// Updates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Update(ScheduledTask instance);

        /// <summary>
        /// Deletes the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        void Delete(ScheduledTask instance);

        /// <summary>
        /// Deletes all scheduled task instances.
        /// </summary>
        void Delete();

        /// <summary>
        /// Finds scheduled tasks.
        /// </summary>
        /// <returns>A query to access scheduled tasks.</returns>
        IQueryable<ScheduledTask> Find();
    }
}