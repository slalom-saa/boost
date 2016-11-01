namespace Slalom.Boost.VisualStudio.RuntimeBinding
{
    /// <summary>
    /// Indicates the binding type for the contract.
    /// </summary>
    public enum BindingType
    {
        /// <summary>
        /// Indicates that no binding type has been set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates there should only be one registration for the contract.
        /// </summary>
        Single,

        /// <summary>
        /// Indicates there should only be multiple registrations for the contract.
        /// </summary>
        Multiple
    }
}