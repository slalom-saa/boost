using System.Web.Http.ExceptionHandling;
using Autofac;
using Slalom.Boost.Aspects;
using Slalom.Boost.Logging;

namespace Slalom.Boost.WebApi
{
    public class OwinExceptionLogger : ExceptionLogger
    {
        private readonly IComponentContext _container;

        public OwinExceptionLogger(IComponentContext container)
        {
            _container = container;
        }

        public override void Log(ExceptionLoggerContext context)
        {
            if (context?.Exception != null)
            {
                var item = _container.Resolve<ILogger>();
                item.Error($"An exception occurred in the OWIN execution.", context.Exception);
            }
            base.Log(context);
        }
    }
}