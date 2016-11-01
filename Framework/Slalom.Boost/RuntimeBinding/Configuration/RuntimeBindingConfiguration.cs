using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.RuntimeBinding.Configuration
{
    /// <summary>
    /// Used to configure types that should be ignored or included in file based configuration.
    /// </summary>
    public static class RuntimeBindingConfiguration
    {
        /// <summary>
        /// The runtime binding configuration name.
        /// </summary>
        public static string ConfigurationName = ConfigurationManager.AppSettings["RuntimeBinding:Configuration"];

        public static IEnumerable<string> IgnoredTypes
        {
            get
            {
                var ignores = ConfigurationManager.AppSettings["RuntimeBinding:Ignore"];
                if (!String.IsNullOrWhiteSpace(ignores))
                {
                    var types = ignores.Split(';');
                    foreach (var type in types)
                    {
                        yield return type;
                    }
                }
            }
        }
    }
}