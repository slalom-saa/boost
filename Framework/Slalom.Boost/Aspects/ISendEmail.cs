using System;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Defines a contract for sending emails.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface ISendEmail
    {
        /// <summary>
        /// Sends an email to the specified recipients.
        /// </summary>
        /// <param name="subject">The subject of the email.</param>
        /// <param name="body">The body of the email</param>
        /// <param name="recipients">The email addresses of recipients of the email.</param>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="subject"/> argument is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="body"/> argument is null or whitespace.</exception>
        /// <exception cref="ArgumentException">Thrown when the <paramref name="recipients"/> argument is null or empty.</exception>
        void Send(string subject, string body, params string[] recipients);
    }
}