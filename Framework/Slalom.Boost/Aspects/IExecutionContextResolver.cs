using Slalom.Boost.RuntimeBinding;

namespace Slalom.Boost.Aspects
{
    /// <summary>
    /// Defines a contract for resolving an execution context.
    /// </summary>
    /// <remarks>
    /// Each host or application type will need it's own resolver.  For example, the execution context for a web-based
    /// application will be different than that of a Windows-based application.
    /// </remarks>
    [RuntimeBindingContract(ContractBindingType.Single)]
    public interface IExecutionContextResolver
    {
        /// <summary>
        /// Resolves the execution context.
        /// </summary>
        /// <returns>Returns the resolved execution context.</returns>
        ExecutionContext Resolve();
    }
}