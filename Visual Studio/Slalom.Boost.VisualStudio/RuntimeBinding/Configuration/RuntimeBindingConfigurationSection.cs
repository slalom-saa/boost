using System.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class RuntimeBindingConfigurationSection : ConfigurationSection
    {
        [ConfigurationProperty("", IsRequired = true, IsDefaultCollection = true)]
        public AddBindingElementCollection Additions
        {
            get { return (AddBindingElementCollection)this[""]; }
            set { this[""] = value; }
        }

        [ConfigurationProperty("ignoreBindings")]
        public IgnoreBindingElementCollection Ignores
        {
            get { return (IgnoreBindingElementCollection)this["ignoreBindings"]; }
            set { this["ignoreBindings"] = value; }
        }
    }
}