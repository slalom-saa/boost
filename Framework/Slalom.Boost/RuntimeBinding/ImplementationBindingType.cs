namespace Slalom.Boost.RuntimeBinding
{
    /// <summary>
    /// Indicates the binding type for the implementation.
    /// </summary>
    public enum ImplementationBindingType
    {
        /// <summary>
        /// Indicates that no binding type has been set.
        /// </summary>
        None = 0,

        /// <summary>
        /// Indicates there should be only one instance of the implemenation.
        /// </summary>
        Singleton
    }
}