using System;
using System.Collections.Generic;
using Serilog.Core;

namespace Slalom.Boost.Logging
{
    public class LoggingModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new LoggingDestructuringPolicy()).As<IDestructuringPolicy>();
            builder.Register(c => new SerilogLogger(c.Resolve<IConfiguration>(), c.Resolve<IExecutionContextResolver>(), c.Resolve<IEnumerable<IDestructuringPolicy>>())).As<ILogger>();
        }
    }
}