using System.Reflection;
using Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal.GraphBuilders
{
    internal static class GraphNodeFactory
    {
        public static GraphNode Create(GraphNode parent, PropertyInfo accessor, bool isCollection, bool isOwned)
        {
            if (isCollection)
            {
                return new CollectionGraphNode(parent, accessor, isOwned);
            }

            return isOwned
                ? new OwnedEntityGraphNode(parent, accessor)
                : (GraphNode)new AssociatedEntityGraphNode(parent, accessor);
        }
    }
}
