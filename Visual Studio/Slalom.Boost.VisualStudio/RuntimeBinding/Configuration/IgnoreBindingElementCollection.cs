using System.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    [ConfigurationCollection(typeof(IgnoreBindingElement), AddItemName = "ignore")]
    public class IgnoreBindingElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoreBindingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((IgnoreBindingElement)element).Type;
        }
    }
}