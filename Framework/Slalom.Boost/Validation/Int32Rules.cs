namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Contains rules for <see cref="int"/> properties.
    /// </summary>
    public static class Int32Rules
    {
        /// <summary>
        /// Creates a rule to validate that the property value is not less than the specified value.
        /// </summary>
        /// <param name="rule">The current rule.</param>
        /// <param name="value">The value to compare.</param>
        /// <param name="message">The message to return.</param>
        /// <returns>Returns the created rule.</returns>
        public static PropertyRule<int> LessThan(this PropertyRule<int> rule, int value, ValidationMessage message)
        {
            return rule.Then = new PropertyRule<int>(message, (e, b) => e < value);
        }
    }
}