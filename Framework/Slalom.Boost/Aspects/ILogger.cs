using System;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Defines a contract for logging messages.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface ILogger
    {
        /// <summary>
        /// Writes the specified debug message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="data"/> argument is null.</exception>
        void Debug(object message, params object[] data);

        /// <summary>
        /// Writes the specified error message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="data"/> argument is null.</exception>
        void Error(object message, params object[] data);

        /// <summary>
        /// Writes the specified information message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="data"/> argument is null.</exception>
        void Information(object message, params object[] data);

        /// <summary>
        /// Writes the specified warning message.
        /// </summary>
        /// <param name="message">The message to write.</param>
        /// <param name="data">Additional data to write to the log.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="data"/> argument is null.</exception>
        void Warning(object message, params object[] data);
    }
}