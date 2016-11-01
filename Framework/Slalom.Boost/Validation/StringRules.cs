using System;
using Slalom.Boost.Domain;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Contains common <see cref="string"/> rules.
    /// </summary>
    public static class StringRules
    {
        /// <summary>
        /// Creates a not null or empty rule.
        /// </summary>
        /// <param name="rule">The current property rule.</param>
        /// <param name="message">The message to add if the instance is empty.</param>
        /// <returns>Returns the created rule.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rule"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        public static PropertyRule<string> NotNullOrEmpty(this PropertyRule<string> rule, ValidationMessage message)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return rule.Then = new PropertyRule<string>(message, (a, b) => !string.IsNullOrEmpty(a));
        }

        /// <summary>
        /// Creates a not null or whitespace rule.
        /// </summary>
        /// <param name="rule">The current property rule.</param>
        /// <param name="message">The message to add if the instance is empty.</param>
        /// <returns>Returns the created rule.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rule"/> argument is null.</exception>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="message"/> argument is null.</exception>
        public static PropertyRule<string> NotNullOrWhiteSpace(this PropertyRule<string> rule, ValidationMessage message)
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }
            return rule.Then = new PropertyRule<string>(message, (a, b) => !string.IsNullOrWhiteSpace(a));
        }

        /// <summary>
        /// Creates a valid rule properties that self-validate.
        /// </summary>
        /// <param name="rule">The current property rule.</param>
        /// <returns>Returns the created rule.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="rule"/> argument is null.</exception>
        public static void Valid<TConcept>(this PropertyRule<string> rule) where TConcept : ConceptAs<string>
        {
            if (rule == null)
            {
                throw new ArgumentNullException(nameof(rule));
            }
            rule.Then = new ConceptPropertyRule<string, TConcept>();
        }
    }
}