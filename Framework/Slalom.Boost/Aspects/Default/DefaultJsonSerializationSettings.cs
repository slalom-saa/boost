using System;
using Newtonsoft.Json;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects.Default
{
    /// <summary>
    /// The default <see cref="JsonSerializerSettings"/>.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.JsonSerializerSettings" />
    [DefaultBinding(Warn = false)]
    public class DefaultJsonSerializationSettings : JsonSerializerSettings
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultJsonSerializationSettings"/> class.
        /// </summary>
        public DefaultJsonSerializationSettings()
        {
            this.ContractResolver = new JsonPrivateMemberContractResolver();
        }
    }
}