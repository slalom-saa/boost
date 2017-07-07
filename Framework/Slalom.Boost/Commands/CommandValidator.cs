using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Autofac;
using Slalom.Boost.Configuration;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Executes standard command validation: input validation, security validation and business validation.
    /// </summary>
    /// <seealso cref="Slalom.Boost.Commands.ICommandValidator" />
    public class CommandValidator : ICommandValidator
    {
        private readonly IComponentContext _container;

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandValidator"/> class.
        /// </summary>
        /// <param name="container">The current <see cref="IContainer"/> instance.</param>
        public CommandValidator(IComponentContext container)
        {
            if (container == null)
            {
                throw new ArgumentNullException(nameof(container));
            }
            _container = container;
        }

        /// <summary>
        /// Validates the specified command.
        /// </summary>
        /// <typeparam name="TResponse">The type of the response expected for the command.</typeparam>
        /// <param name="command">The command to validate.</param>
        /// <param name="context">The current command context.</param>
        /// <returns>The <see cref="ValidationMessage">messages</see> returned from validation routines.</returns>
        /// <exception cref="System.ArgumentNullException"></exception>
        public IEnumerable<ValidationMessage> Validate<TResponse>(Command<TResponse> command, CommandContext context)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }
            var target = this.CheckInputRules(command, context).ToList();
            if (target.Any())
            {
                return target;
            }
            foreach (var result in this.CheckSecurityRules(command, context))
            {
                target.Add(result);
                if (target.Any())
                {
                    return target;
                }
            }
            foreach (var result in this.CheckBusinessRules(command, context))
            {
                target.Add(result);
                if (target.Any())
                {
                    return target;
                }
            }
            return target.AsEnumerable();
        }

        private IEnumerable<ValidationMessage> CheckBusinessRules<TResponse>(Command<TResponse> command, CommandContext context)
        {
            var type = typeof(IBusinessValidationRule<>).MakeGenericType(command.GetType());

            var sets = _container.ResolveAll(type);

            var method = typeof(IValidationRule<,>).MakeGenericType(command.GetType(), typeof(CommandContext)).GetMethod("Validate");

            return sets.OrderBy(e => ((dynamic)e).Order)
                       .SelectMany(e => (IEnumerable<ValidationMessage>)method.Invoke(e, new object[] { command, context }))
                       .Select(e => e.WithType(ValidationMessageType.Business));
        }

        private IEnumerable<ValidationMessage> CheckInputRules<TResponse>(Command<TResponse> command, CommandContext context)
        {
            var type = typeof(IInputValidationRule<>).MakeGenericType(command.GetType());
            var sets = _container.ResolveAll(type);

            var method = typeof(IValidationRule<,>).MakeGenericType(command.GetType(), typeof(CommandContext)).GetMethod("Validate");

            return sets.SelectMany(e => (IEnumerable<ValidationMessage>)method.Invoke(e, new object[] { command, context }))
                       .Select(e => e.WithType(ValidationMessageType.Input));
        }

        private IEnumerable<ValidationMessage> CheckSecurityRules<TResponse>(Command<TResponse> command, CommandContext context)
        {
            var type = typeof(ISecurityValidationRule<>).MakeGenericType(command.GetType());
            var sets = _container.ResolveAll(type);

            var method = typeof(IValidationRule<,>).MakeGenericType(command.GetType(), typeof(CommandContext)).GetMethod("Validate");

            return sets.SelectMany(e => (IEnumerable<ValidationMessage>)method.Invoke(e, new object[] { command, context }))
                       .Select(e => e.WithType(ValidationMessageType.Security));
        }
    }
}