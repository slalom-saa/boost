using System;
using Newtonsoft.Json;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// The default <see cref="JsonSerializerSettings"/>.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonSerializerSettings" />
    public class DefaultJsonSerializationSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializationSettings"/> class.
        /// </summary>
        public DefaultJsonSerializationSettings()
        {
            this.ContractResolver = new JsonPrivateMemberContractResolver();

            JsonConvert.DefaultSettings = () => new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            };
        }
    }
}