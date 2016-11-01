using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;

namespace Slalom.Boost
{
    /// <summary>
    /// Provides consistent validation routines and messaging.
    /// </summary>
    /// <remarks>
    /// If you are having trouble debugging this class makes sure that exceptions are turned on.  Go to DEBUG -> Exceptions... Then check
    /// Thrown in Common Language Runtime Exceptions.
    /// </remarks>
    public static class Argument
    {
        /// <summary>
        /// Validates that the argument is not null.
        /// </summary>
        /// <typeparam name="TProperty">The type of the argument.</typeparam>
        /// <param name="expression">The argument as an expression.</param>
        /// <returns>Returns the passed in expression for chaining.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the argument could not be validated because the expression is null.</exception>
        /// <exception cref="System.ArgumentNullException">Thrown when the argument is null.</exception>
        /// <example>
        /// To use in a method, call the validation method with the argument.
        /// <code>
        /// public void Method(string text)
        /// {
        ///     Argument.NotNull(() =&gt; text);
        /// }
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Convenience method.")]
        public static Expression<Func<TProperty>> NotNull<TProperty>(this Expression<Func<TProperty>> expression)
        {
            if (expression == null)
            {
                throw new InvalidOperationException("The argument could not be validated because the expression is null.");
            }
            var value = expression.Compile()();
            if (Equals(value, default(TProperty)))
            {
                throw new ArgumentNullException(expression.GetPropertyName());
            }

            return expression;
        }

        /// <summary>
        /// Validates that the argument is not null or empty.
        /// </summary>
        /// <param name="expression">The argument as an expression.</param>
        /// <returns>Returns the passed in expression for chaining.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the argument could not be validated because the expression is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the argument is null or white space.</exception>
        /// <example>
        /// To use in a method, call the validation method with the argument.
        /// <code>
        /// public void Method(IEnumerable&lt;T> text)
        /// {
        ///     Argument.NotNullOrEmpty(() =&gt; text);
        /// }
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Convenience method.")]
        public static Expression<Func<IEnumerable<object>>> NotNullOrEmpty(this Expression<Func<IEnumerable<object>>> expression)
        {
            if (expression == null)
            {
                throw new InvalidOperationException("The argument could not be validated because the expression is null.");
            }
            var value = expression.Compile()();
            if (value == null || !value.Any())
            {
                var name = expression.GetPropertyName();
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "The argument {0} must be not be null and must have values.", name), name);
            }
            return expression;
        }

        /// <summary>
        /// Validates that the argument is not null or whitespace.
        /// </summary>
        /// <param name="expression">The argument as an expression.</param>
        /// <returns>Returns the passed in expression for chaining.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the argument could not be validated because the expression is null.</exception>
        /// <exception cref="System.ArgumentException">Thrown when the argument is null or white space.</exception>
        /// <example>
        /// To use in a method, call the validation method with the argument.
        /// <code>
        /// public void Method(string text)
        /// {
        ///     Argument.NotNullOrWhiteSpace(() =&gt; text);
        /// }
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Convenience method.")]
        public static Expression<Func<string>> NotNullOrWhiteSpace(this Expression<Func<string>> expression)
        {
            if (expression == null)
            {
                throw new InvalidOperationException("The argument could not be validated because the expression is null.");
            }
            var value = expression.Compile()();
            if (string.IsNullOrWhiteSpace(value))
            {
                var name = expression.GetPropertyName();
                throw new ArgumentException(string.Format(CultureInfo.InvariantCulture,
                    "The argument {0} must be a non-empty value.  The passed in value is \"{1}\".", name, value), name);
            }
            return expression;
        }

        /// <summary>
        /// Validates that the argument is greater than.
        /// </summary>
        /// <param name="expression">The argument as an expression.</param>
        /// <param name="comparison">The number to compare.</param>
        /// <returns>Returns the passed in expression for chaining.</returns>
        /// <exception cref="System.InvalidOperationException">Thrown when the argument could not be validated because the expression is null.</exception>
        /// <example>
        /// To use in a method, call the validation method with the argument.
        /// <code>
        /// public void Method(int number)
        /// {
        ///     Argument.GreaterThan(() =&gt; number, 0);
        /// }
        /// </code></example>
        [SuppressMessage("Microsoft.Design", "CA1006:DoNotNestGenericTypesInMemberSignatures", Justification = "Convenience method.")]
        public static Expression<Func<int>> GreaterThan(this Expression<Func<int>> expression, int comparison)
        {
            if (expression == null)
            {
                throw new InvalidOperationException("The argument could not be validated because the expression is null.");
            }
            var value = expression.Compile()();
            if (value <= comparison)
            {
                var name = expression.GetPropertyName();

                throw new ArgumentOutOfRangeException(name, $"The argument {name} must be greater than {comparison}.  The passed in value is \"{value}\".");
            }
            return expression;
        }

        private static string GetPropertyName<TProperty>(this Expression<Func<TProperty>> expression)
        {
            var memberExpression = (MemberExpression)expression.Body;
            return memberExpression.Member.Name;
        }
    }
}