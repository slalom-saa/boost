using System;
using Newtonsoft.Json;

namespace Slalom.Boost.Serialization
{
    /// <summary>
    /// Specifies the default messaging settings.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonSerializerSettings" />
    public class DefaultSerializationSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultSerializationSettings"/> class.
        /// </summary>
        public DefaultSerializationSettings()
        {
            this.Formatting = Formatting.Indented;
            this.ContractResolver = new DefaultContractResolver();
        }

        /// <summary>
        /// Gets an instance of the settings.
        /// </summary>
        /// <value>An instance of the settings.</value>
        public static JsonSerializerSettings Instance => new DefaultSerializationSettings();
    }
}