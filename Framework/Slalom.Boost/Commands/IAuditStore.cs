using System.Linq;
using System.Threading.Tasks;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Defines a contract for saving <see cref="Command{TResponse}">commands</see> and <see cref="CommandResult">results</see> as an Audit.
    /// </summary>
    public interface IAuditStore
    {
        /// <summary>
        /// Gets a value indicating whether this instance can read.
        /// </summary>
        /// <value><c>true</c> if this instance can read; otherwise, <c>false</c>.</value>
        bool CanRead { get; }

        /// <summary>
        /// Saves the executed <see cref="Command{TResponse}">command</see> and <see cref="CommandResult">result</see>.
        /// </summary>
        /// <typeparam name="TResponse">The type of response.</typeparam>
        /// <param name="command">The executing command.</param>
        /// <param name="result">The <see cref="CommandResult">command execution result</see>.</param>
        /// <returns>A <see cref="Task" /> that saves the executed <see cref="Command{TResponse}">command</see> and <see cref="CommandResult">result</see>.</returns>
        Task SaveAsync<TResponse>(Command<TResponse> command, CommandResult<TResponse> result);

        /// <summary>
        /// Finds all <see cref="CommandAudit"/> instances.
        /// </summary>
        IQueryable<CommandAudit> Find();
    }
}