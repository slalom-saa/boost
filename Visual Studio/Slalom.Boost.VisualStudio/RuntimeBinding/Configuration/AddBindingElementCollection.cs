using System.Configuration;

namespace Slalom.Boost.VisualStudio.RuntimeBinding.Configuration
{
    public class AddBindingElementCollection : ConfigurationElementCollection
    {
        protected override ConfigurationElement CreateNewElement()
        {
            return new RuntimeBindingElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return ((RuntimeBindingElement)element).Type;
        }
    }
}