using System;

namespace Slalom.Boost.Serialization
{
    /// <summary>
    /// Indicates that a property should be handled securely when logged.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class SecureAttribute : Attribute
    {
        public const string DefaultText = "[SECURE]";
    }
}