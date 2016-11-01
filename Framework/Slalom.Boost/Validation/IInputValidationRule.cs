using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Defines a contract for input validation of a command.
    /// </summary>
    /// <typeparam name="TValue">The type of the command to validate.</typeparam>
    public interface IInputValidationRule<in TValue> : IValidationRule<TValue, CommandContext> where TValue : ICommand
    {
    }
}