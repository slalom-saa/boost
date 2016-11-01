using System;
using System.Linq.Expressions;

namespace Slalom.Boost.AutoMapper.QueryableExtensions
{
    public class ExpressionResolutionResult
    {
        public Expression ResolutionExpression { get; private set; }
        public Type Type { get; private set; }

        public ExpressionResolutionResult(Expression resolutionExpression, Type type)
        {
            this.ResolutionExpression = resolutionExpression;
            this.Type = type;
        }
    }
}