using System.Collections.Generic;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Defines a contract for a self-validating class.
    /// </summary>
    public interface IValidate
    {
        /// <summary>
        /// Validates this instance.
        /// </summary>
        /// <returns>
        /// Returns validation errors that were found as a result of the validation.
        /// An empty collection means that no errors were found.
        /// </returns>
        IEnumerable<ValidationMessage> Validate();
    }
}