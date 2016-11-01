using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Represents a rule for self-validating classes.
    /// </summary>
    /// <typeparam name="TValue">The type of instance to validate.</typeparam>
    /// <seealso cref="Slalom.Boost.Validation.PropertyRule{TValue}" />
    public class ValidateRule<TValue> : PropertyRule<TValue> where TValue : IValidate
    {
        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="context">The current context that can be used to share information
        /// between validation rules.</param>
        /// <returns>Returns all found validation errors.</returns>
        public override IEnumerable<ValidationMessage> Validate(TValue instance, CommandContext context)
        {
            var target = instance.Validate();
            if (target.Any())
            {
                return target;
            }
            if (Then != null)
            {
                return Then.Validate(instance, context);
            }
            return Enumerable.Empty<ValidationMessage>();
        }
    }
}