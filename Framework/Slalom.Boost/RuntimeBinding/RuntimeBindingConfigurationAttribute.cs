using System;

namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Used to specify a configuration for a runtime binding implementation.
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [Serializable, AttributeUsage(AttributeTargets.Class)]
    public sealed class RuntimeBindingConfigurationAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RuntimeBindingConfigurationAttribute"/> class.
        /// </summary>
        /// <param name="name">The configuration name.</param>
        public RuntimeBindingConfigurationAttribute(string name)
        {
            this.Name = name;
        }

        /// <summary>
        /// Gets the configuration name.
        /// </summary>
        /// <value>The configuration name.</value>
        public string Name { get; private set; }
    }
}