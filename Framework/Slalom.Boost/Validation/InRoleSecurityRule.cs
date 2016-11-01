using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Checks to see if a user is in one of the specified roles.
    /// </summary>
    /// <typeparam name="TCommand">The type of the command.</typeparam>
    public class InRoleSecurityRule<TCommand> : ISecurityValidationRule<TCommand> where TCommand : ICommand
    {
        private readonly string _message;
        private readonly string[] _roles;

        /// <summary>
        /// Initializes a new instance of the <see cref="InRoleSecurityRule{TCommand}"/> class.
        /// </summary>
        /// <param name="message">The message to display on invalid.</param>
        /// <param name="roles">The roles to check for.</param>
        public InRoleSecurityRule(string message, params string[] roles)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                throw new ArgumentException(nameof(message));
            }
            if (roles == null || roles.All(string.IsNullOrWhiteSpace))
            {
                throw new ArgumentException(nameof(roles));
            }
            _roles = roles;
            _message = message;
        }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns all found validation messages.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> argument is null.</exception>
        public virtual IEnumerable<ValidationMessage> Validate(TCommand instance, CommandContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (context.Identity is WindowsIdentity)
            {
            }
            else
            {
                var identity = context.Identity as ClaimsIdentity;
                if (identity == null || !_roles.Any(role => identity.Claims.Any(e => e.Type == ClaimTypes.Role && e.Value == role)))
                {
                    yield return _message;
                }
            }
        }
    }
}