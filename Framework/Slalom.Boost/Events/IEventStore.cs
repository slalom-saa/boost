using System;
using System.Linq;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Events
{
    /// <summary>
    /// Defines a contract for logging application messages.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Multiple)]
    public interface IEventStore : IHandleEvent
    {
        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        bool CanRead { get; }

        /// <summary>
        /// Appends the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="context">The current <see cref="T:Slalom.Boost.Commands.CommandContext" /> instance.</param>
        void Append(Event instance, CommandContext context);

        /// <summary>
        /// Finds all <see cref="Event"/> instances.
        /// </summary>
        /// <returns>An IQueryable&lt;Event&gt; that can be used to filter and project.</returns>
        IQueryable<EventAudit> Find();
    }
}