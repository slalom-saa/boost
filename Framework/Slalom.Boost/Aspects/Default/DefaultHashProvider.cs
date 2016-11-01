using System;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// Provides a default <see cref="IHashProvider"/> implementation.
    /// </summary>
    /// <seealso cref="IHashProvider" />
    [DefaultBinding(Warn = false)]
    public class DefaultHashProvider : IHashProvider
    {
        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="salt">The salt to use.</param>
        public string Hash(string text, string salt)
        {
            return Encryption.Hash(text, salt);
        }

        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        public string Hash(string text)
        {
            return Encryption.Hash(text);
        }
    }
}
