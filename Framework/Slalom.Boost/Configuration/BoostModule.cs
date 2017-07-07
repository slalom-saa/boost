using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Newtonsoft.Json;
using Slalom.Boost.Aspects;
using Slalom.Boost.Aspects.Default;
using Slalom.Boost.Commands;
using Slalom.Boost.Domain;
using Slalom.Boost.Domain.Default;
using Slalom.Boost.Events;
using Slalom.Boost.Logging;
using Slalom.Boost.ReadModel.Default;

namespace Slalom.Boost.Configuration
{
    public class BoostModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            base.Load(builder);

            builder.Register(c => new DefaultJsonSerializationSettings()).As<JsonSerializerSettings>();
            builder.Register(c => new AutoMapperMapper()).As<IMapper>();
            builder.Register(c => new SmtpEmailService()).As<ISendEmail>();
            builder.Register(c => new DefaultHashProvider()).As<IHashProvider>();
            builder.Register(c => new CommandValidator(c.Resolve<IComponentContext>())).As<ICommandValidator>();

            builder.RegisterType<EventPublisher>().As<IEventPublisher>();
            builder.RegisterType<AggregateFacade>().As<IAggregateFacade>();
            builder.RegisterGeneric(typeof(InMemoryRepository<>));
            builder.RegisterType<LoggingDestructuringPolicy>().AsImplementedInterfaces();
            builder.RegisterType<EventHandlerResolver>().AsImplementedInterfaces();
            builder.RegisterType<InMemoryReadModelFacade>().AsImplementedInterfaces();
            builder.RegisterType<WindowsExecutionContextResolver>().AsImplementedInterfaces();
        }
    }
}

