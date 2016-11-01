using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Represents a rule that validates a property on a class instance.
    /// </summary>
    /// <typeparam name="TValue">The type of value to validate.</typeparam>
    public class PropertyRule<TValue> : IValidationRule<TValue, CommandContext>
    {
        private readonly ValidationMessage _message;

        private readonly Func<TValue, CommandContext, bool> _validation;
        internal PropertyRule<TValue> Then;

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRule{TValue}"/> class.
        /// </summary>
        /// <param name="message">The message to return if the rule is not met.</param>
        /// <param name="validation">The validation function.</param>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="validation"/> argument is null.</exception>
        public PropertyRule(ValidationMessage message, Func<TValue, CommandContext, bool> validation)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            if (validation == null)
            {
                throw new ArgumentNullException(nameof(validation));
            }
            _message = message;
            _validation = validation;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PropertyRule{TValue}"/> class.
        /// </summary>
        protected PropertyRule()
        {
        }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="context">The current context that can be used to share information
        /// between validation rules.</param>
        /// <returns>Returns all found validation errors.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="context"/> argument is null.</exception>
        public virtual IEnumerable<ValidationMessage> Validate(TValue instance, CommandContext context)
        {
            if (!_validation(instance, context))
            {
                yield return _message;
            }
            else if (Then != null)
            {
                foreach (var error in Then.Validate(instance, context))
                {
                    yield return error;
                }
            }
        }
    }
}