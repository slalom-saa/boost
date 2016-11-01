using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Represents an input validation rule set that should be run as a single unit.
    /// </summary>
    /// <typeparam name="TValue">The type of command.</typeparam>
    /// <seealso cref="Slalom.Boost.Validation.IInputValidationRule{TValue}" />
    public class InputValidationRuleSet<TValue> : IInputValidationRule<TValue> where TValue : ICommand
    {
        private readonly List<IValidationRule<TValue, CommandContext>> _rules = new List<IValidationRule<TValue, CommandContext>>();

        /// <summary>
        /// Adds the specified property rule.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to validate.</typeparam>
        /// <param name="property">The property to validate.</param>
        /// <returns>Returns the starter rule for fluent validation.</returns>
        public PropertyRule<TProperty> Add<TProperty>(Expression<Func<TValue, TProperty>> property)
        {
            var target = new PropertyRuleCollection<TValue, TProperty>(property);

            _rules.Add(target);

            return target.StarterRule;
        }

        /// <summary>
        /// Adds the specified property rule.
        /// </summary>
        /// <typeparam name="TProperty">The type of the property to validate.</typeparam>
        /// <param name="property">The property to validate.</param>
        /// <param name="action">The action.</param>
        /// <returns>Returns the starter rule for fluent validation.</returns>
        public void Add<TProperty>(Expression<Func<TValue, IEnumerable<TProperty>>> property, Action<PropertyRule<TProperty>> action)
        {
            var target = new EnumerablePropertyRuleCollection<TValue, TProperty>(property, action);

            _rules.Add(target);
        }

        /// <summary>
        /// Validates the specified instance.
        /// </summary>
        /// <param name="instance">The instance to validate.</param>
        /// <param name="context">The current context.</param>
        /// <returns>Returns all found validation messages.</returns>
        /// <exception cref="ArgumentNullException">Thrown when the <paramref name="instance"/> argument is null.</exception>
        public virtual IEnumerable<ValidationMessage> Validate(TValue instance, CommandContext context)
        {
            if (instance == null)
            {
                throw new ArgumentNullException(nameof(instance));
            }
            return _rules.SelectMany(e => e.Validate(instance, context)).Select(e => e.WithType(ValidationMessageType.Input));
        }
    }
}