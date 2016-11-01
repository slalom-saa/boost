using System;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Defines a common interface for commands.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1040:AvoidEmptyInterfaces")]
    public interface ICommand : IHaveIdentity
    {
    }
}