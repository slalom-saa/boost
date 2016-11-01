using System;
using Machine.Specifications;
using Moq;
using Slalom.Boost;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace $safeprojectname$.Application.given
{
    public class a_configured_container
    {
        private static Lazy<IContainer> container = new Lazy<IContainer>(() => new SimpleContainer(typeof(a_configured_container)));

        private static Lazy<Mock<IDataFacade>> dataFacade = new Lazy<Mock<IDataFacade>>(() =>
        {
            var instance = new Mock<IDataFacade>();
            Container.Register<IDataFacade>(instance.Object);
            return instance;
        });

        protected static IMessageBus Bus => Container.Resolve<IMessageBus>();
        protected static Mock<IDataFacade> DataFacade => dataFacade.Value;
        protected static IContainer Container => container.Value;
    }
}
