using Microsoft.Practices.Unity;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.RuntimeBinding.Configuration;

namespace Slalom.Boost.WebApi
{
    public static class UnityConfiguration
    {
        public static IContainer AutoConfigure(this UnityContainer container, params BindingFilter[] filters)
        {
            return ((IUnityContainer)container).AutoConfigure(filters);
        }

        public static IContainer AutoConfigure(this IUnityContainer container, params BindingFilter[] filters)
        {
            var adapter = new UnityContainerAdapter(container);

            var configuration = new RuntimeBindingConfigurator(filters);

            configuration.ConfigureContainer(adapter);

            return adapter;
        }
    }
}