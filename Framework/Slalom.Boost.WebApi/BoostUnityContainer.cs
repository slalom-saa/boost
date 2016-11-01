using System;
using Microsoft.Practices.Unity;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.WebApi
{
    public class BoostUnityContainer : UnityContainer
    {
        private bool _disposed;

        public BoostUnityContainer()
        {
        }

        public BoostUnityContainer(object parent)
        {
            this.AutoConfigure(parent);
        }

        public BoostUnityContainer(Type type)
        {
            this.AutoConfigure(type);
        }

        public IApplicationBus Bus
        {
            get { return this.Resolve<IApplicationBus>(); }
        }

        protected override void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                _disposed = true;

                base.Dispose(disposing);
            }
        }
    }

    public static class UnityExtensions
    {
        public static IContainer AutoConfigure(this IUnityContainer container, object parent)
        {
            return new UnityContainerAdapter(container).AutoConfigure(parent.GetType());
        }

        public static IContainer AutoConfigure(this IUnityContainer container, Type type)
        {
            return new UnityContainerAdapter(container).AutoConfigure(type);
        }
    }
}