using System;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Defines a contract for hashing text.
    /// </summary>
    public interface IHashProvider
    {
        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="salt">The salt to use.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="text"/> or <paramref name="salt"/> is null or whitespace.</exception>
        string Hash(string text, string salt);

        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException">Thrown when the <paramref name="text"/> is null or whitespace.</exception>
        string Hash(string text);
    }
}
