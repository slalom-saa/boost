using Slalom.Boost.Commands;

namespace Slalom.Boost.Validation
{
    /// <summary>
    /// Defines a contract for security validation rules.
    /// </summary>
    /// <typeparam name="TCommand">The type of instance to validate.</typeparam>
    public interface ISecurityValidationRule<in TCommand> : IValidationRule<TCommand, CommandContext> where TCommand : ICommand
    {
    }
}