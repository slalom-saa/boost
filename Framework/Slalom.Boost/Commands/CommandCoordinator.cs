using System;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Autofac;
using Slalom.Boost.Aspects;
using Slalom.Boost.Configuration;
using Slalom.Boost.Logging;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Coordinates the handling of a command, passing it through the command stack.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Commands.ICommandCoordinator" />
    public class CommandCoordinator : ICommandCoordinator
    {
        protected readonly IComponentContext Container;
        protected readonly ICommandValidator Validator;
        private readonly Lazy<ILogger> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandCoordinator"/> class.
        /// </summary>
        /// <param name="container">The current <see cref="IContainer"/> instance.</param>
        /// <param name="validator">The current <see cref="ICommandValidator"/> instance.</param>
        /// <exception cref="System.ArgumentNullException">
        /// Thrown when the <paramref name="container"/> or <paramref name="validator"/> argument are null.
        /// </exception>
        public CommandCoordinator(IComponentContext container, ICommandValidator validator)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            if (validator == null)
            {
                throw new ArgumentNullException(nameof(validator));
            }
            Container = container;
            Validator = validator;
            _logger = new Lazy<ILogger>(() => Container.Resolve<ILogger>());
        }

        /// <summary>
        /// Handles the validation and execution of the specified command.
        /// </summary>
        /// <param name="command">The command to handle.</param>
        /// <param name="context"></param>
        /// <returns>
        /// A command result with validation errors and data.
        /// </returns>
        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed.")]
        public virtual Task<CommandResult<TResponse>> Handle<TResponse>(Command<TResponse> command, CommandContext context)
        {
            Argument.NotNull(() => command);
            Argument.NotNull(() => context);

            return Task<CommandResult<TResponse>>.Factory.StartNew(() =>
            {
                _logger.Value.Verbose("Starting execution for " + command.GetType() + ". {@Command}", command);

                var result = CommandResult<TResponse>.Start(command, context);
                try
                {
                    this.Validate(command, result);

                    context.CancellationToken.ThrowIfCancellationRequested();

                    if (!result.ValidationMessages.Any())
                    {
                        this.ExecuteHandler(command, result);
                    }
                }
                catch (ValidationException exception)
                {
                    result.AddValidationMessages(exception.ValidationMessages);
                }
                catch (AggregateException exception)
                {
                    if (exception.InnerException is ValidationException)
                    {
                        result.AddValidationMessages(((ValidationException)exception.InnerException).ValidationMessages);
                    }
                    else if (exception.InnerException is TargetInvocationException)
                    {
                        this.LogException(((TargetInvocationException)exception.InnerException).InnerException, command, context);
                        result.SetException(((TargetInvocationException)exception.InnerException).InnerException);
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (TargetInvocationException exception)
                {
                    this.LogException(exception.InnerException, command, context);
                    result.SetException(exception.InnerException);
                }
                catch (Exception exception)
                {
                    this.LogException(exception, command, context);
                    result.SetException(exception);
                }

                try
                {
                    this.Complete(command, result);
                }
                catch (TargetInvocationException exception)
                {
                    this.LogException(exception.InnerException, command, context);
                    result.SetException(exception.InnerException);
                }
                catch (Exception exception)
                {
                    this.LogException(exception, command, context);
                    result.SetException(exception);
                }

                return result;
            }, context.CancellationToken);
        }

        protected virtual void Complete<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            result.Complete();

            this.PublishRaisedEvents(result);

            this.SaveToCommandStores(command, result);

            if (!result.Successful)
            {
                if (result.Exception != null)
                {
                    _logger.Value.Error(result.Exception, "An unhandled exception was raised while executing " + command.GetType().Name + ". {@Command} {@Context}", command, result.Context);
                }
                else if (result.ValidationMessages?.Any() ?? false)
                {
                    _logger.Value.Verbose("Execution completed with validation errors while executing " + command.GetType().Name + ". {@Command} {@ValidationErrors} {@Context}", command, result.ValidationMessages, result.Context);
                }
                else
                {
                    _logger.Value.Error("Execution completed unsuccessfully while executing " + command.GetType().Name + ". {@Command} {@Result} {@Context}", command, result, result.Context);
                }
            }
            else
            {
                _logger.Value.Verbose("Successfully completed " + command.GetType().Name + ". {@Command} {@Result} {@Context}", command, result, result.Context);
            }
        }

        protected virtual void ExecuteHandler<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            var commandType = command.GetType();
            var baseType = commandType.BaseType;
            if (baseType != null && baseType.IsGenericType)
            {
                var dataType = baseType.GetGenericArguments()[0];

                var handlerType = typeof(IHandleCommand<,>).MakeGenericType(commandType, dataType);
                var handler = (IHaveCommandContext)Container.Resolve(handlerType);
                if (handler != null)
                {
                    handler.SetContext(result.Context);
                    result.Response = ((dynamic)handler).HandleCommand((dynamic)command);
                }
                else
                {
                    var bus = Container.Resolve<IServiceBus>();
                    if (bus == null)
                    {
                        throw new InvalidOperationException($"A command handler for the specified type ({command.GetType()}) could not be found.");
                    }

                    result.Response = bus.Send(command, result.Context).Result.Response;
                }
            }
        }

        [SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "As designed.")]
        protected virtual void LogException<TResult>(Exception exception, Command<TResult> command, CommandContext context)
        {
            var logger = Container.Resolve<ILogger>();
            try
            {
                logger.Error(exception, "An unhandled exception occurred while executing a command. {@Command} {@Context}", command, context);
            }
            catch (Exception caught)
            {
                Trace.TraceError(caught.ToString());
            }
        }

        protected virtual void PublishRaisedEvents<TResponse>(CommandResult<TResponse> response)
        {
            if (response.CommandId == response.Context.CommandTrace.First())
            {
                var bus = Container.Resolve<IApplicationBus>();
                bus.Publish(response.Context.RaisedEvents, response.Context).Wait();
            }
        }

        protected virtual void SaveToCommandStores<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            var stores = Container.ResolveAll<IAuditStore>();

            var tasks = stores.Select(store => store.SaveAsync(command, result));

            Task.WhenAll(tasks).Wait();
        }

        protected virtual void Validate<TResponse>(Command<TResponse> command, CommandResult<TResponse> result)
        {
            var results = Validator.Validate(command, result.Context);
            if (results.Any())
            {
                result.AddValidationMessages(results);
            }
        }
    }
}