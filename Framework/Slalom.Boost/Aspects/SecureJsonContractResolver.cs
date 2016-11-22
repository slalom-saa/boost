using System;
using System.Reflection;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using Slalom.Boost.Serialization;
using DefaultContractResolver = Newtonsoft.Json.Serialization.DefaultContractResolver;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// A JSON Contract Resolver that ignores sensitive members.
    /// </summary>
    /// <seealso cref="Newtonsoft.Json.Serialization.DefaultContractResolver" />
    public class SecureJsonContractResolver : DefaultContractResolver
    {
        /// <summary>
        /// Creates a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.
        /// </summary>
        /// <param name="member">The member to create a <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for.</param>
        /// <param name="memberSerialization">The member's parent <see cref="T:Newtonsoft.Json.MemberSerialization" />.</param>
        /// <returns>A created <see cref="T:Newtonsoft.Json.Serialization.JsonProperty" /> for the given <see cref="T:System.Reflection.MemberInfo" />.</returns>
        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var prop = base.CreateProperty(member, memberSerialization);
            if ((member as PropertyInfo)?.GetCustomAttributes<SecureAttribute>().Any() ?? false)
            {
                prop.Converter = new SecureJsonConverter();
            }
            return prop;
        }
    }
}