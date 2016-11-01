using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace Slalom.Boost.EntityFramework.GraphDiff.Internal
{
    /// <summary>Db load queries</summary>
    internal interface IQueryLoader
    {
        T LoadEntity<T>(T entity, IEnumerable<string> includeStrings, QueryMode queryMode) where T : class;
        T LoadEntity<T>(Expression<Func<T, bool>> keyPredicate, IEnumerable<string> includeStrings, QueryMode queryMode) where T : class;
    }
}