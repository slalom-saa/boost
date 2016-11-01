using System.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class IgnoreBindingElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }
    }
}