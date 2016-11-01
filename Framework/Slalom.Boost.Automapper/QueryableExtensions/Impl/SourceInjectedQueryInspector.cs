using System;
using System.Linq.Expressions;

namespace Slalom.Boost.AutoMapper.QueryableExtensions.Impl
{
    public class SourceInjectedQueryInspector
    {
        public SourceInjectedQueryInspector()
        {
            this.SourceResult = (e,o) => { };
            this.DestResult = o => { };
            this.StartQueryExecuteInterceptor = (t, e) => { };
        }
        public Action<Expression, object> SourceResult { get; set; }
        public Action<object> DestResult { get; set; }
        public Action<Type, Expression> StartQueryExecuteInterceptor { get; set; }

    }
}