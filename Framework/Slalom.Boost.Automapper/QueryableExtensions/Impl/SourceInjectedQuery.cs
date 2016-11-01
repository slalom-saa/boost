using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Slalom.Boost.AutoMapper.QueryableExtensions.Impl
{
    public class SourceInjectedQuery<TSource, TDestination> : IOrderedQueryable<TDestination>
    {
        public SourceInjectedQuery(IQueryable<TSource> dataSource, IQueryable<TDestination> destQuery, IMapper mapper, SourceInjectedQueryInspector inspector = null)
        {
            this.Expression = destQuery.Expression;
            this.ElementType = typeof(TDestination);
            this.Provider = new SourceInjectedQueryProvider<TSource, TDestination>(mapper, dataSource, destQuery)
            {
                Inspector = inspector ?? new SourceInjectedQueryInspector()
            };
        }

        internal SourceInjectedQuery(IQueryProvider provider, Expression expression)
        {
            this.Provider = provider;
            this.Expression = expression;
            this.ElementType = typeof(TDestination);
        }

        public IEnumerator<TDestination> GetEnumerator()
        {
            return this.Provider.Execute<IEnumerable<TDestination>>(this.Expression).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        public Type ElementType { get; }
        public Expression Expression { get; }
        public IQueryProvider Provider { get; }
    }


}
