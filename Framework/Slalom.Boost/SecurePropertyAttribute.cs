using System;
using System.Linq;

namespace Slalom.Boost
{
    /// <summary>
    /// Indicates that a property should be handled securely in transit and when persisted.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SecurePropertyAttribute : Attribute
    {
        /// <summary>
        /// The default secure property text.
        /// </summary>
        public static string DefaultText = "[SECURE]";
    }
}