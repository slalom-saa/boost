using System;
using System.Linq;
using System.Security.Principal;
using System.Web;
using Slalom.Boost.Aspects;

namespace Slalom.Boost.WebApi
{
    public class WebApiExecutionContextResolver : IExecutionContextResolver
    {
        public ExecutionContext Resolve()
        {
            if (HttpContext.Current == null)
            {
                var target = new ExecutionContext(new GenericIdentity("Not Authenticated"), "None");
                target.Data.Add("Error", "Invalid execution context.  Make sure to unregister this type if not running in an HTTP context.");
                return target;
            }
            else
            {
                var target = new ExecutionContext(HttpContext.Current.User?.Identity, HttpContext.Current.Request.Headers.AllKeys.Contains("Session") ? HttpContext.Current.Request.Headers["Session"] : null);
                target.Data.Add("Browser", HttpContext.Current.Request.Browser?.Browser);
                target.Data.Add("URL", HttpContext.Current.Request.Url.AbsoluteUri);
                target.Data.Add("Request IP Address", HttpContext.Current.Request.UserHostAddress);
                target.Data.Add("Host", "HTTP");
                return target;
            }
        }
    }
}
