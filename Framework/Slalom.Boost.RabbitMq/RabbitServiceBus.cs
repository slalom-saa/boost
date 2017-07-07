using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using MassTransit;
using MassTransit.RabbitMqTransport;
using Slalom.Boost.Commands;
using Slalom.Boost.Events;

namespace Slalom.Boost.RabbitMq
{
    public class RabbitServiceBus : IServiceBus
    {
        protected static readonly string HostUri = ConfigurationManager.AppSettings["Messaging:RabbitMQHost"] ?? "rabbitmq://localhost";
        protected static readonly string CommandQueueName = ConfigurationManager.AppSettings["Messaging:CommandQueueName"] ?? "commands";
        protected static readonly string EventQueueName = ConfigurationManager.AppSettings["Messaging:EventQueueName"] ?? "events";
        protected static readonly string UserName = ConfigurationManager.AppSettings["Messaging:Username"] ?? "guest";
        protected static readonly string Password = ConfigurationManager.AppSettings["Messaging:Password"] ?? "guest";

        private readonly List<Guid> _handledEvents = new List<Guid>();
        private bool _started;
        protected IBusControl Bus;

        public RabbitServiceBus()
        {
            Bus = global::MassTransit.Bus.Factory.CreateUsingRabbitMq(configurator =>
            {
                var host = configurator.Host(new Uri(HostUri), configure =>
                {
                    configure.Username(UserName);
                    configure.Password(Password);
                });

                configurator.UseBinarySerializer();

                this.ConfigureReceiveEndpoints(configurator, host);
            });

            this.Start();
        }

        public virtual async Task<CommandResult<TResponse>> Send<TResponse>(Command<TResponse> instance, CommandContext context)
        {
            this.Start();

            var serviceUri = new Uri(HostUri + "/" + CommandQueueName);

            var method = typeof(RequestClientExtensions).GetMethod("CreateRequestClient", BindingFlags.Static | BindingFlags.Public);
            method = method.MakeGenericMethod(typeof(CommandEnvelop), typeof(ResponseEnvelope));

            var client = method.Invoke(null, new object[] { Bus, serviceUri, TimeSpan.FromSeconds(30), null, null });

            var result = (ResponseEnvelope)await ((dynamic)client).Request(new CommandEnvelop(instance, context), CancellationToken.None);

            var target = result.GetResponse();

            return (CommandResult<TResponse>)target;
        }

        public virtual async Task Publish(IEvent instance, CommandContext context)
        {
            await Bus.Publish(new EventEnvelope(instance, context));
        }

        public virtual async Task Publish(IEnumerable<IEvent> instances, CommandContext context)
        {
            await Task.WhenAll(instances.Select(e => this.Publish(e, context)));
        }

        public void Dispose()
        {
            this.Stop();
        }

        protected virtual void ConfigureReceiveEndpoints(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
        }

        protected void ConfigureCommandsEndpoint(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
            configurator.ReceiveEndpoint(host, CommandQueueName, configure =>
            {
                configure.Handler<CommandEnvelop>(async handler =>
                {
                    using (var container = new ApplicationContainer(this))
                    {
                        if (container.CanResolve(typeof(IHandleCommand<,>).MakeGenericType(handler.Message.CommandType, handler.Message.CommandType.BaseType.GetGenericArguments()[0])))
                        {
                            var command = handler.Message.GetCommand();
                            var commandContext = handler.Message.GetContext();

                            var result = await container.Bus.Send((dynamic)command, commandContext);

                            await handler.RespondAsync(new ResponseEnvelope(result));
                        }
                    }
                });
            });
        }

        protected void ConfigureEventsEndpoint(IRabbitMqBusFactoryConfigurator configurator, IRabbitMqHost host)
        {
            configurator.ReceiveEndpoint(host, EventQueueName, configure =>
            {
                configure.Handler<EventEnvelope>(async handler =>
                {
                    var instance = handler.Message.GetEvent();
                    if (_handledEvents.Contains(instance.Id))
                    {
                        return;
                    }
                    _handledEvents.Add(instance.Id);

                    using (var container = new ApplicationContainer(this))
                    {
                        var publisher = container.Resolve<IEventPublisher>();

                        await publisher.Publish(handler.Message.GetEvent(), handler.Message.GetContext());
                    }
                });
            });
        }

        protected virtual void Start()
        {
            if (!_started)
            {
                _started = true;
                Bus.Start();
            }
        }

        protected virtual void Stop()
        {
            if (_started)
            {
                Bus?.Stop();
            }
        }
    }
}