using System.Web.Http.ExceptionHandling;
using Slalom.Boost.Aspects;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.WebApi
{
    public class OwinExceptionLogger : ExceptionLogger
    {
        private readonly IContainer _container;

        public OwinExceptionLogger(IContainer container)
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