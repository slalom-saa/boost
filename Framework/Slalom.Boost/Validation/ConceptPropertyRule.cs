using System.Collections.Generic;
using System.Linq;
using Slalom.Boost.Commands;
using Slalom.Boost.Domain;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Represents a <see cref="ConceptAs{TValue}"/> property rule.
    /// </summary>
    /// <typeparam name="TValue">The type of the instance to validate.</typeparam>
    /// <typeparam name="TConcept">The type of the concept.</typeparam>
    /// <seealso cref="Slalom.Boost.Validation.PropertyRule{TValue}" />
    public class ConceptPropertyRule<TValue, TConcept> : PropertyRule<TValue> where TConcept : ConceptAs<TValue>
    {
        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance.</param>
        /// <param name="context">The context.</param>
        /// <returns>IEnumerable&lt;ValidationMessage&gt;.</returns>
        public override IEnumerable<ValidationMessage> Validate(TValue instance, CommandContext context)
        {
            try
            {
                var messages = ((TConcept)(dynamic)instance);
            }
            catch (ValidationException exception)
            {
                return exception.ValidationMessages;
            }
            if (Then != null)
            {
                return Then.Validate(instance, context);
            }
            return Enumerable.Empty<ValidationMessage>();
        }
    }
}