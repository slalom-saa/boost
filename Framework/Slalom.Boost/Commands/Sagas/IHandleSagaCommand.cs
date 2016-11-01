using System.CodeDom.Compiler;
using System.Diagnostics;

namespace Slalom.Boost.Commands.Sagas
{
    /// <summary>
    /// Defines a contract for handling command from a saga.
    /// </summary>
    /// <typeparam name="TCommand">The type of the t command.</typeparam>
    /// <typeparam name="TData">The type of the t data.</typeparam>
    /// <seealso cref="Slalom.Boost.Commands.IHandleCommand{TCommand, TData}" />
    public interface IHandleSagaCommand<TCommand, TData> : IHandleCommand<TCommand, TData> where TCommand : Command<TData>
    {
        ISaga Find(TCommand instance);
    }
}