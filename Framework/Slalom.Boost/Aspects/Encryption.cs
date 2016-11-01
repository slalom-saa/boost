using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Provides simplified methods for encrypting and decrypting text.
    /// </summary>
    public static class Encryption
    {
        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            using (var provider = Activator.CreateInstance<T>())
            {
                return Decrypt(text, key, provider);
            }
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            return Decrypt(text, key, null);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <example>
        ///   <c>Encryption.Decrypt("encrypted content");</c>
        /// </example>
        public static string Decrypt(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return Decrypt(text, null, null);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// An encrypted string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text, string key)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            return Encrypt(text, null, key);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// An encrypted string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            using (var provider = Activator.CreateInstance<T>())
            {
                return Encrypt(text, provider, key);
            }
        }

        /// <summary>
        /// Encrypts the specified text using a default encryption provider.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>
        /// An encrypted string.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            return Encrypt(text, null, null);
        }

        /// <summary>
        /// Creates a hash from the from the specified content using the specified salt.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <param name="salt">The string to use for a salt.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">
        /// </exception>
        public static string Hash(string text, string salt)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }
            if (string.IsNullOrEmpty(salt))
            {
                throw new ArgumentException("The provided salt cannot be null or empty.", nameof(salt));
            }

            return Hash(salt + Hash(text));
        }

        /// <summary>
        /// Creates a hash from the specified text.
        /// </summary>
        /// <param name="text">The text to hash.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static string Hash(string text)
        {
            if (string.IsNullOrEmpty(text))
            {
                throw new ArgumentException("The provided text cannot be null or empty.", nameof(text));
            }

            using (var provider = new MD5CryptoServiceProvider())
            {
                var data = provider.ComputeHash(Encoding.Default.GetBytes(text));
                return Convert.ToBase64String(data);
            }
        }

        #region Non-Public Methods

        private static void SetValidKey(this SymmetricAlgorithm provider, string key)
        {
            key = key ?? "";
            provider.GenerateKey();
            provider.Key = Encoding.ASCII.GetBytes(key.Resize(provider.Key.Length, ' '));
        }

        [SuppressMessage("ReSharper", "InconsistentNaming")]
        private static void SetValidIV(this SymmetricAlgorithm provider, string key)
        {
            key = key ?? "";
            provider.GenerateIV();
            provider.IV = Encoding.ASCII.GetBytes(key.Resize(provider.IV.Length, ' '));
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static string Encrypt(string text, SymmetricAlgorithm provider, string key)
        {
            string target = null;
            bool created = false;
            try
            {
                if (provider == null)
                {
                    provider = new RijndaelManaged();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);


                using (var memoryStream = new MemoryStream())
                {
                    var content = Encoding.ASCII.GetBytes(text);
                    var cryptoStream = new CryptoStream(memoryStream, provider.CreateEncryptor(), CryptoStreamMode.Write);
                    cryptoStream.Write(content, 0, content.Length);
                    cryptoStream.FlushFinalBlock();
                    target = Convert.ToBase64String(memoryStream.ToArray());
                }
            }
            finally
            {
                if (created)
                {
                    provider.Dispose();
                }
            }
            return target;
        }

        [SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        private static string Decrypt(string text, string key, SymmetricAlgorithm provider)
        {
            string target = null;
            bool created = false;
            try
            {
                if (provider == null)
                {
                    provider = new RijndaelManaged();
                    created = true;
                }
                provider.SetValidKey(key);
                provider.SetValidIV(key);

                var content = Convert.FromBase64String(text);

                using (var memoryStream = new MemoryStream(content, 0, content.Length))
                {
                    var cryptoStream = new CryptoStream(memoryStream, provider.CreateDecryptor(), CryptoStreamMode.Read);
                    using (var reader = new StreamReader(cryptoStream))
                    {
                        target = reader.ReadToEnd();
                    }
                }
            }
            finally
            {
                if (created)
                {
                    provider.Dispose();
                }
            }
            return target;
        }

        #endregion
    }
}