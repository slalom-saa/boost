using System.Collections.Generic;
using System.Text.RegularExpressions;
using Slalom.Boost.Validation;

namespace Slalom.Boost.Domain
{
    /// <summary>
    /// Represents a phone number <see href="https://www.safaribooksonline.com/library/view/microsoft-net-architecting/9780133986419/ch08.html#ch08lev2sec3">Value Object</see>.
    /// </summary>
    public class PhoneNumber : ConceptAs<string>
    {
        private const string Expression = @"^(\+\d{1,2}\s)?\(?\d{3}\)?[\s.-]\d{3}[\s.-]\d{4}$";

        private static readonly Regex Regex = new Regex(Expression, RegexOptions.Compiled);

        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>Returns validation errors that were found as a result of the validation.
        /// An empty collection means that no errors were found.</returns>
        public override IEnumerable<ValidationMessage> Validate()
        {
            if (this.Value == null || !Regex.IsMatch(this.Value))
            {
                yield return new ValidationMessage("PhoneNumber.Valid", "A phone number must have a valid format.");
            }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="System.String"/> to <see cref="PhoneNumber"/>.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator PhoneNumber(string value)
        {
            var target = new PhoneNumber { Value = value };
            target.EnsureValid();
            return target;
        }
    }
}