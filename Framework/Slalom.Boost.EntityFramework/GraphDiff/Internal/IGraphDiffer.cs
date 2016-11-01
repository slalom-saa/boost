namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    internal interface IGraphDiffer<T> where T : class
    {
        T Merge(T updating, QueryMode queryMode = QueryMode.SingleQuery);
    }
}