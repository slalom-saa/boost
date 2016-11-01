using System;
using System.Security.Claims;
using System.Web;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace Slalom.Boost.WebApi
{
    public class AuthorizeClaims : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            var identity = HttpContext.Current.User.Identity as ClaimsIdentity;

            return identity != null && identity.IsAuthenticated;
        }
    }
}