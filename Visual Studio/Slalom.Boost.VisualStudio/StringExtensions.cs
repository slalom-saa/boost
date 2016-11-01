using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity.Design.PluralizationServices;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Slalom.Boost.VisualStudio.Dynamic;
using Slalom.Boost.VisualStudio.Humanizer;

namespace Slalom.Boost.VisualStudio
{
    /// <summary>
    /// Contains extensions for <see cref="String"/> objects.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Returns a copy of the string in camel case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in camel case.</returns>
        public static string ToCamelCase(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.Substring(0, 1).ToLowerInvariant() + instance.Substring(1);
        }

        /// <summary>
        /// Returns a copy of the string pluralized.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string pluralized.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Pluralize(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return PluralizationService.CreateService(CultureInfo.CurrentCulture).Pluralize(instance);
        }

        /// <summary>
        /// Returns a copy of the string singularized.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string singularized.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public static string Singularize(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }

            return PluralizationService.CreateService(CultureInfo.CurrentCulture).Singularize(instance);
        }

        /// <summary>
        /// Returns a copy of the string in pascal case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in pascal case.</returns>
        public static string ToPascalCase(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return instance.Substring(0, 1).ToUpperInvariant() + instance.Substring(1);
        }

        /// <summary>
        /// Resizes the specified string.
        /// </summary>
        /// <param name="text">The specified string.</param>
        /// <param name="count">The desired length.</param>
        /// <param name="pad">The pad character if needed.</param>
        /// <returns>A String of the specified length.</returns>
        public static string Resize(this string text, int count, char pad)
        {
            if (text == null)
            {
                throw new ArgumentNullException(nameof(text));
            }

            return new string(text.Take(count).ToArray()).PadRight(count, pad);
        }

        /// <summary>
        /// Returns a copy of the string with tokens replaced.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="replacements">An object containing replacements.</param>
        /// <returns></returns>
        public static string ReplaceTokens(this string instance, object replacements, params Type[] references)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            if (replacements == null)
            {
                throw new ArgumentNullException(nameof(replacements));
            }
            var target = new Dictionary<string, object>();

            var dictionary = TypeDescriptor.GetProperties(replacements.GetType()).Cast<PropertyDescriptor>().ToDictionary(property => property.Name,
                property => property.GetValue(replacements));

            foreach (var item in dictionary)
            {
                target[item.Key] = item.Value;
            }

            return Regex.Replace(instance,
                @"{{(.*?)}}",
                m =>
                {
                    var key = target.Keys.FirstOrDefault(e => e.Equals(m.Groups[1].Value, StringComparison.OrdinalIgnoreCase));
                    if (key != null)
                    {
                        return Convert.ToString(target[key]);
                    }
                    if (m.Groups[1].Value.Contains("."))
                    {
                        try
                        {
                            var interpreter = new Interpreter()
                                .Reference(typeof(StringHumanizeExtensions))
                                .Reference(typeof(LetterCasing))
                                .Reference(typeof(StringExtensions));

                            references.ToList().ForEach(e =>
                            {
                                interpreter.Reference(e);
                            });

                            interpreter.SetVariable("instance", replacements);

                            return interpreter.Parse("instance." + m.Groups[1].Value).Invoke().ToString();
                        }
                        catch
                        {
                            Trace.TraceError($"Failed to parse {m.Groups[1].Value}");
                        }
                    }
                    return m.Groups[0].Value;
                },
                RegexOptions.IgnoreCase);
        }

        /// <summary>
        /// Returns a copy of the string in snake case.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns a copy of the string in snake case.</returns>
        public static string ToSnakeCase(this string instance)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return string.Concat(instance.Select((x, i) => i > 0 && char.IsUpper(x) ? "_" + x.ToString() : x.ToString())).ToLowerInvariant();
        }

        public static string Uncompress(this string instance)
        {
            var bytes = Convert.FromBase64String(instance);
            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(msi, CompressionMode.Decompress))
                    {
                        gs.CopyTo(mso);
                    }

                    return Encoding.UTF8.GetString(mso.ToArray());
                }
            }
        }

        public static string Compress(this string instance)
        {
            var bytes = Encoding.UTF8.GetBytes(instance);

            using (var msi = new MemoryStream(bytes))
            {
                using (var mso = new MemoryStream())
                {
                    using (var gs = new GZipStream(mso, CompressionMode.Compress))
                    {
                        msi.CopyTo(gs);
                    }

                    return Convert.ToBase64String(mso.ToArray());
                }
            }
        }
    }
}