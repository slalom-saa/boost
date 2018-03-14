/* 
 * Copyright (c) Patolus Contributors
 * 
 * This file is subject to the terms and conditions defined in
 * the LICENSE file, which is part of this source code package.
 */

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
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt<T>(byte[] text, string key) where T : SymmetricAlgorithm, new()
        {
            using (var provider = Activator.CreateInstance<T>())
            {
                return DecryptInternal(text, key);
            }
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            using (var provider = Activator.CreateInstance<T>())
            {
                return DecryptInternal(text, key);
            }
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Decrypt(string text, string key)
        {
            return DecryptInternal(text, key);
        }

        /// <summary>
        /// Decrypts the specified text with the specified provider and key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The decrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt(byte[] text, string key)
        {
            return DecryptInternal(text, key);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        /// <example>
        ///   <c>Encryption.Decrypt("encrypted content");</c>
        /// </example>
        public static string Decrypt(string text)
        {
            return DecryptInternal(text, null);
        }

        /// <summary>
        /// Decrypts the specified text.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>The decrypted text.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Decrypt(byte[] text)
        {

            return DecryptInternal(text, null);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text, string key)
        {
            return EncryptInternal(text, key);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt(byte[] text, string key)
        {
            return EncryptInternal(text, key);
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt<T>(string text, string key) where T : SymmetricAlgorithm, new()
        {
            using (var provider = Activator.CreateInstance<T>())
            {
                return EncryptInternal(text, key);
            }
        }

        /// <summary>
        /// Encrypts the specified text with specified key.
        /// </summary>
        /// <typeparam name="T">The type of SymmetricAlgorithm to use.</typeparam>
        /// <param name="text">The specified text.</param>
        /// <param name="key">The specified key.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt<T>(byte[] text, string key) where T : SymmetricAlgorithm, new()
        {
            using (var provider = Activator.CreateInstance<T>())
            {
                return EncryptInternal(text, key);
            }
        }

        /// <summary>
        /// Encrypts the specified text using a default encryption provider.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Encrypt(string text)
        {
            return EncryptInternal(text, null);
        }

        /// <summary>
        /// Encrypts the specified text using a default encryption provider.
        /// </summary>
        /// <param name="text">The specified text.</param>
        /// <returns>
        /// The encrypted text.
        /// </returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static byte[] Encrypt(byte[] text)
        {
            return EncryptInternal(text, null);
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
            using (var provider = MD5.Create())
            {
                var data = provider.ComputeHash(Encoding.UTF8.GetBytes(text));
                return Convert.ToBase64String(data);
            }
        }

        #region Non-Public Methods

        private static byte[] EncryptInternal(byte[] text, string key)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.Key = Encoding.ASCII.GetBytes(key.Resize(aesAlg.Key.Length, ' '));

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            return combinedIvCt;
        }

        private static string EncryptInternal(string text, string key)
        {
            byte[] encrypted;
            byte[] IV;

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.Key = Encoding.ASCII.GetBytes(key.Resize(aesAlg.Key.Length, ' '));

                aesAlg.GenerateIV();
                IV = aesAlg.IV;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for encryption. 
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            //Write all data to the stream.
                            swEncrypt.Write(text);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            var combinedIvCt = new byte[IV.Length + encrypted.Length];
            Array.Copy(IV, 0, combinedIvCt, 0, IV.Length);
            Array.Copy(encrypted, 0, combinedIvCt, IV.Length, encrypted.Length);

            return Convert.ToBase64String(combinedIvCt);
        }

        private static string DecryptInternal(string text, string key)
        {
            var cipherTextCombined = Convert.FromBase64String(text);

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.Key = Encoding.ASCII.GetBytes(key.Resize(aesAlg.Key.Length, ' '));

                byte[] IV = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];

                Array.Copy(cipherTextCombined, IV, IV.Length);
                Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return plaintext;
        }

        private static byte[] DecryptInternal(byte[] text, string key)
        {
            var cipherTextCombined = text;

            // Declare the string used to hold 
            // the decrypted text. 
            string plaintext = null;

            // Create an Aes object 
            // with the specified key and IV. 
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                aesAlg.Key = Encoding.ASCII.GetBytes(key.Resize(aesAlg.Key.Length, ' '));

                byte[] IV = new byte[aesAlg.BlockSize / 8];
                byte[] cipherText = new byte[cipherTextCombined.Length - IV.Length];

                Array.Copy(cipherTextCombined, IV, IV.Length);
                Array.Copy(cipherTextCombined, IV.Length, cipherText, 0, cipherText.Length);

                aesAlg.IV = IV;

                aesAlg.Mode = CipherMode.CBC;

                // Create a decrytor to perform the stream transform.
                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                // Create the streams used for decryption. 
                using (var msDecrypt = new MemoryStream(cipherText))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {

                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = srDecrypt.ReadToEnd();
                        }
                    }
                }

            }

            return Encoding.UTF8.GetBytes(plaintext);
        }

        #endregion
    }
}