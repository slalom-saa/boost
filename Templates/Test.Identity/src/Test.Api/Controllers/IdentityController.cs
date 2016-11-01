using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Test.Api.Controllers
{
    [Route("identity"), Authorize]
    public class IdentityController : ControllerBase
    {
        [HttpGet]
        public dynamic Get()
        {
            return new JsonResult(from c in this.User.Claims select new { c.Type, c.Value });
        }
    }
}
