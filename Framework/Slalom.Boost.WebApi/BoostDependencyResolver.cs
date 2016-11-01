using System;
using System.Collections.Generic;
using System.Web.Http.Dependencies;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.WebApi
{
    public class BoostDependencyResolver : IDependencyResolver
    {
        public IContainer Container { get; set; }

        public BoostDependencyResolver(IContainer container)
        {
            this.Container = container;
        }

        public IDependencyScope BeginScope()
        {
            return new BoostDependencyScope(this.Container.CreateChildContainer());
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return this.Container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.ResolveAll(serviceType);
        }
    }


    public class BoostDependencyScope : IDependencyScope
    {
        public IContainer Container { get; set; }

        public BoostDependencyScope(IContainer container)
        {
            this.Container = container;
        }

        public void Dispose()
        {
            this.Container.Dispose();
        }

        public object GetService(Type serviceType)
        {
            return this.Container.Resolve(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return this.Container.ResolveAll(serviceType);
        }
    }
}