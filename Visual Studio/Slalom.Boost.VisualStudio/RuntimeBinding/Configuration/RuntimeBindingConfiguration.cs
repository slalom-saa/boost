using System.Collections.Generic;
using System.Configuration;
using System.Linq;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public static class RuntimeBindingConfiguration
    {
        static RuntimeBindingConfiguration()
        {
            configurationSection = (RuntimeBindingConfigurationSection)ConfigurationManager.GetSection("runtimeBinding");
        }

        private static readonly RuntimeBindingConfigurationSection configurationSection;

        public static AddBindingElementCollection Additions = configurationSection?.Additions ?? new AddBindingElementCollection();

        public static IEnumerable<IgnoreBindingElement> Ignores = configurationSection?.Ignores.OfType<IgnoreBindingElement>() ?? Enumerable.Empty<IgnoreBindingElement>();
    }
}