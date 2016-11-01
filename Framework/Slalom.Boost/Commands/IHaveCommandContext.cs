using System;

namespace Slalom.Boost.Commands
{
    /// <summary>
    /// Defines a contract for having a <see cref="CommandContext"/>.
    /// </summary>
    public interface IHaveCommandContext
    {
        /// <summary>
        /// Gets or sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <value>The current <see cref="CommandContext"/>.</value>
        CommandContext Context { get; }

        /// <summary>
        /// Sets the current <see cref="CommandContext"/>.
        /// </summary>
        /// <param name="context">The current <see cref="CommandContext"/>.</param>
        void SetContext(CommandContext context);
    }
}