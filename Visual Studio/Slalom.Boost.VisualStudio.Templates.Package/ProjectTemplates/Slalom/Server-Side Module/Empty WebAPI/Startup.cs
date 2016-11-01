using System.Collections.Generic;
using Slalom.Boost.RuntimeBinding.Configuration;
using Microsoft.Owin;
using $safeprojectname$;
using Slalom.Boost.WebApi;
using System;

[assembly: OwinStartup(typeof (Startup))]

namespace $safeprojectname$
{
    public class Startup : BoostApiBoostrapper
    {
    }
}