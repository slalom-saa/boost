using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    internal class EnumerablePropertyRuleCollection<TValue, TProperty> : IValidationRule<TValue, CommandContext> where TValue : ICommand
    {
        private readonly Expression<Func<TValue, IEnumerable<TProperty>>> _property;
        private readonly Action<PropertyRule<TProperty>> _action;

        internal PropertyRule<TProperty> StarterRule = new PropertyRule<TProperty>("This is a starter rule that can be ignored.", (a, b) => true);

        public EnumerablePropertyRuleCollection(Expression<Func<TValue, IEnumerable<TProperty>>> property, Action<PropertyRule<TProperty>> action)
        {
            _property = property;
            _action = action;
        }

        public IEnumerable<ValidationMessage> Validate(TValue instance, CommandContext context)
        {
            var value = _property.Compile()(instance);

            _action.Invoke(StarterRule);

            foreach (var item in value)
            {
                var target = StarterRule.Validate(item, context);
                if (target.Any())
                {
                    return target;
                }
            }
            return Enumerable.Empty<ValidationMessage>();
        }
    }
}