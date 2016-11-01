using System.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class RuntimeBindingElement : ConfigurationElement
    {
        [ConfigurationProperty("type", IsKey = true, IsRequired = true)]
        public string Type
        {
            get { return (string)base["type"]; }
            set { base["type"] = value; }
        }

        [ConfigurationProperty("mapTo", IsRequired = true)]
        public string MapTo
        {
            get { return (string)base["mapTo"]; }
            set { base["mapTo"] = value; }
        }

        [ConfigurationProperty("assembly", IsRequired = false)]
        public string Assembly
        {
            get { return (string)base["assembly"]; }
            set { base["assembly"] = value; }
        }
    }
}