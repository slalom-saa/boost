using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Slalom.Boost.Commands;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Provides a base class for security validation rules.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    /// <seealso cref="Slalom.Boost.Validation.ISecurityValidationRule{TCommand}" />
    public abstract class SecurityValidationRule<TCommand> : IHaveDataFacade,
                                                             IHaveCommandContext,
                                                             ISecurityValidationRule<TCommand> where TCommand : ICommand

    {
        /// <summary>
        /// Gets or sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <value>The current <see cref="CommandContext"/>.</value>
        public CommandContext Context { get; private set; }

        /// <summary>
        /// Sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CommandContext"/>.</param>
        public void SetContext(CommandContext context)
        {
            this.Context = context;
        }

        /// <summary>
        /// Gets or sets the current <see cref="IDataFacade"/>.
        /// </summary>
        /// <value>The current <see cref="IDataFacade"/>.</value>
        [RuntimeBindingDependency]
        public IDataFacade DataFacade { get; protected set; }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The command instance.</param>
        /// <param name="context">The current command context.</param>
        /// <returns>Returns all found validation messages.</returns>
        public IEnumerable<ValidationMessage> Validate(TCommand instance, CommandContext context)
        {
            this.Context = context;

            return this.Validate(instance);
        }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <returns>Returns all found validation messages.</returns>
        public abstract IEnumerable<ValidationMessage> Validate(TCommand instance);

        /// <summary>
        /// Determines whether the calling user is in the specified role.
        /// </summary>
        /// <param name="role">The role to check.</param>
        /// <returns>Returns <c>true</c> if the calling user is in the specified role; otherwise, <c>false</c>.</returns>
        protected bool IsInRole(string role)
        {
            var identity = this.Context.Identity as ClaimsIdentity;
            if (identity == null || !identity.Claims.Any(e => e.Type == ClaimTypes.Role && e.Value == role))
            {
                return false;
            }
            return true;
        }
    }
}