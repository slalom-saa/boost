using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Indicates that a property value should be resolved when a class is resolved.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Property)]
    public sealed class RuntimeBindingDependencyAttribute : Attribute
    {
    }
}