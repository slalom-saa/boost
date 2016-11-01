using System.Configuration;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Used to configure types that should be ignored or included in file based configuration.
    /// </summary>
    /// <seealso cref="System.Configuration.ConfigurationElement" />
    public class RuntimeBindingElement : ConfigurationElement
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        /// <summary>
        /// Gets or sets the type that should be mapped to.
        /// </summary>
        /// <value>The type that should be mapped to.</value>
        [ConfigurationProperty("mapTo", IsRequired = true)]
        public string MapTo
        {
            get { return (string)base["mapTo"]; }
            set { base["mapTo"] = value; }
        }
    }
}