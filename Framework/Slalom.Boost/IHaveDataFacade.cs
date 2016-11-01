using System;

namespace Slalom.Boost
{
    /// <summary>
    /// Defines a contract for a class that has an <see cref="IDataFacade"/>.
    /// </summary>
    public interface IHaveDataFacade
    {
        /// <summary>
        /// Gets the current <see cref="IDataFacade"/> instance.
        /// </summary>
        /// <value>The current <see cref="IDataFacade"/> instance.</value>
        IDataFacade DataFacade { get; }
    }
}