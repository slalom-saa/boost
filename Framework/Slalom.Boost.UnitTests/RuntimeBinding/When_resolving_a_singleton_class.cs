using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Machine.Specifications;
using Slalom.Boost.EntityFramework.Extensions;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.UnitTests.RuntimeBinding
{
    public class When_dispoing_a_container_with_a_disposable_class
    {
        public class DisposableClass : IDisposable
        {
            public bool Disposed { get; private set; } = false;

            public void Dispose()
            {
                this.Disposed = true;
            }
        }

        Establish context = () =>
        {
            container = new ApplicationContainer();
            container.Register<DisposableClass, DisposableClass>();
            instance = container.Resolve<DisposableClass>();
        };

        static ApplicationContainer container;

        Because of = () =>
        {
            container.Dispose();
        };

        private It should_be_disposed = () => instance.Disposed.ShouldBeTrue();

        private static DisposableClass instance;
    }

    public class When_resolving_a_singleton_class
    {
        [RuntimeBindingImplementation(ImplementationBindingType.Singleton)]
        public class SingletonImplemenation
        {
        }

        Establish context = () =>
        {
            container = new ApplicationContainer();
            container.Register<SingletonImplemenation, SingletonImplemenation>();
        };

        static ApplicationContainer container;

        Because of = () =>
        {
            instance1 = container.Resolve<SingletonImplemenation>();
            instance2 = container.Resolve<SingletonImplemenation>();
        };

        private static SingletonImplemenation instance1;
        private static SingletonImplemenation instance2;

        It should_be_the_same_instance = () => instance1.ShouldEqual(instance2);
    }

    public class When_resolving_a_non_singleton_class
    {
        [RuntimeBindingImplementation(ImplementationBindingType.None)]
        public class NonSingletonImplemenation
        {
        }

        Establish context = () =>
        {
            container = new ApplicationContainer();
            container.Register<NonSingletonImplemenation, NonSingletonImplemenation>();
        };

        static ApplicationContainer container;

        Because of = () =>
        {
            instance1 = container.Resolve<NonSingletonImplemenation>();
            instance2 = container.Resolve<NonSingletonImplemenation>();
        };

        private static NonSingletonImplemenation instance1;
        private static NonSingletonImplemenation instance2;

        It should_not_be_the_same_instance = () => instance1.ShouldNotEqual(instance2);
    }
}
