using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.Practices.Unity;
using Slalom.Boost.Aspects;
using Slalom.Boost.Commands;
using Slalom.Boost.Domain;
using Slalom.Boost.Events;
using Slalom.Boost.RuntimeBinding;
using Slalom.Boost.Validation;

namespace Slalom.Boost.WebApi.Controllers
{
    /// <summary>
    /// Provides a base configured API Controller.
    /// </summary>
    /// <seealso cref="System.Web.Http.ApiController" />
    public class CommandApiController : ApiController
    {
        /// <summary>
        /// Gets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IDataFacade"/> instance.
        /// </value>
        [Dependency]
        public IDataFacade DataFacade { set; get; }

        /// <summary>
        /// Gets the current <see cref="IApplicationBus"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IApplicationBus"/> instance.
        /// </value>
        [Dependency]
        public IApplicationBus Bus { set; get; }

        /// <summary>
        /// Gets the current <see cref="IMapper"/> instance.
        /// </summary>
        /// <value>
        /// The current <see cref="IMapper"/> instance.
        /// </value>
        [Dependency]
        public IMapper Mapper { set; get; }

        /// <summary>
        /// Sends the specified command and creates a response based on the result.
        /// </summary>
        /// <typeparam name="TCommand">The type of the command.</typeparam>
        /// <param name="command">The command to send.</param>
        /// <returns>A response message based on the command result.</returns>
        protected async Task<dynamic> SendAsync<TCommand>(TCommand command) where TCommand : ICommand
        {
            if (command == null)
            {
                return Task.FromResult<dynamic>(this.Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            CommandResult result = await this.Bus.Send((dynamic)command);

            if (result.Successful)
            {
                var target = ((dynamic)result).Response;
                if (target is Event)
                {
                    return this.Request.CreateResponse(HttpStatusCode.OK);
                }
                else
                {
                    return target;
                }
            }

            return this.HandleError(result);
        }

        /// <summary>
        /// Sends the specified command and creates a response based on the result.
        /// </summary>
        /// <param name="command">The command to send.</param>
        /// <returns>A response message based on the command result.</returns>
        protected async Task<dynamic> SendAsync<TResponse>(Command<TResponse> command, Func<TResponse, dynamic> value)
        {
            if (command == null)
            {
                return Task.FromResult<dynamic>(this.Request.CreateResponse(HttpStatusCode.BadRequest));
            }

            CommandResult result = await this.Bus.Send((dynamic)command);

            if (result.Successful)
            {
                var target = ((dynamic)result).Response;

                return value(target);
            }

            return this.HandleError(result);
        }

        protected dynamic HandleError(CommandResult result)
        {
            // throw an exception if it was raised
            if (result.Exception != null)
            {
                if (result.Exception is ValidationException)
                {
                    return this.Request.CreateResponse(HttpStatusCode.BadRequest, ((ValidationException)result.Exception).ValidationMessages);
                }

                throw result.Exception;
            }

            // return a forbidden response if a security validation message was returned
            if (result.ValidationMessages.Any(x => x.MessageType == ValidationMessageType.Security))
            {
                return this.Request.CreateResponse(HttpStatusCode.Forbidden, result.ValidationMessages);
            }

            // at this point we know the request was bad, so we return any messages
            return this.Request.CreateResponse(HttpStatusCode.BadRequest, result.ValidationMessages);
        }
    }
}