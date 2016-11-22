using System;
using System.Net;
using System.Net.Http;
using System.Web.Http.Filters;
using Slalom.Boost.Aspects;
using Slalom.Boost.Logging;

namespace Slalom.Boost.WebApi
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
    public class WebApiExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IExecutionContextResolver _context;
        private readonly ILogger _logger;

        public WebApiExceptionFilter(IExecutionContextResolver context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        public override void OnException(HttpActionExecutedContext context)
        {
            var correlationId = _context.Resolve().CorrelationId;

            _logger.Error($"An exception occurred in the Web API.", context.Exception);

            if (context.Exception is ArgumentException || context.Exception is InvalidOperationException)
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.BadRequest,
                    new { CorrelationId = correlationId, Message = "The request was invalid.  Please check the request and try again.", context.Exception });
            }
            else
            {
                context.Response = context.Request.CreateResponse(HttpStatusCode.InternalServerError,
                    new { CorrelationId = correlationId, Message = "An unhandled exception occurred on the server.  Please try again.", context.Exception });
            }
        }
    }
}