using System;
using System.Linq;
using System.Web.Http;
using System.Web.Http.OData;
using Slalom.Boost.WebApi.Controllers;
using System.Threading.Tasks;
using Slalom.Boost.Commands;

namespace $safeprojectname$.Controllers
{
	// TODO: Remove the AllowAnonymous attribute when roles are in place

    [RoutePrefix("admin"), AllowAnonymous]
    public class AdminController : AdminApiController
    {
        [Route("audits"), HttpGet, EnableQuery]
        public dynamic Audits()
        {
            return base.GetAuditQueries()
                        .FirstOrDefault();
        }

        [Route("events"), HttpGet, EnableQuery]
        public dynamic Events()
        {
            return base.GetEventQueries()
                        .FirstOrDefault();
        }
    }
}