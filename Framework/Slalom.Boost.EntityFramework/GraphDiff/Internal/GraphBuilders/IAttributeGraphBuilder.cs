using Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.GraphBuilders
{
    internal interface IAttributeGraphBuilder
    {
        bool CanBuild<T>();
        GraphNode BuildGraph<T>();
    }
}