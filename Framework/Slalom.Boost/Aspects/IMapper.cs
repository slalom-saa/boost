using System;
using System.Linq;
using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Defines a contract for mapping objects to other objects.
    /// </summary>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IMapper
    {
        /// <summary>
        /// Maps the specified instance to the specified type.
        /// </summary>
        /// <typeparam name="TOut">The type to map to.</typeparam>
        /// <param name="instance">The instance to map.</param>
        /// <returns>Returns the mapped type.</returns>
        TOut Map<TOut>(object instance);

        /// <summary>
        /// Projects the specified <see cref="IQueryable"/> instance to the specified type.
        /// </summary>
        /// <typeparam name="TOut">The type of the project to.</typeparam>
        /// <param name="instance">The instance.</param>
        /// <returns>A query of the specified type.</returns>
        IQueryable<TOut> ProjectTo<TOut>(IQueryable<object> instance);

        /// <summary>
        /// Maps the specified source to the destination type.
        /// </summary>
        /// <param name="source">The source instance.</param>
        /// <param name="destinationType">The destination type.</param>
        /// <returns>Returns the mapped object.</returns>
        object Map(object source, Type destinationType);
    }
}
