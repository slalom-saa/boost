using System;
using System.Linq.Expressions;
using Slalom.Boost.EntityFramework.GraphDiff.Internal.Graph;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    internal interface IAggregateRegister
    {
        void ClearAll();
        void Register<T>(GraphNode rootNode, string scheme = null);
        GraphNode GetEntityGraph<T>(string scheme = null);
        GraphNode GetEntityGraph<T>(Expression<Func<IUpdateConfiguration<T>, object>> expression);
    }
}