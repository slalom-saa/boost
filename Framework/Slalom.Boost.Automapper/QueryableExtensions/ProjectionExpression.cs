using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Slalom.Boost.AutoMapper.Internal;
using IObjectDictionary = System.Collections.Generic.IDictionary<string, object>;

namespace Slalom.Boost.AutoMapper.QueryableExtensions
{
    using MemberPaths = IEnumerable<IEnumerable<MemberInfo>>;

    public class ProjectionExpression : IProjectionExpression
    {
        private static readonly MethodInfo QueryableSelectMethod = FindQueryableSelectMethod();

        private readonly IQueryable _source;
        private readonly IExpressionBuilder _builder;

        public ProjectionExpression(IQueryable source, IExpressionBuilder builder)
        {
            _source = source;
            _builder = builder;
        }

        private static MethodInfo FindQueryableSelectMethod()
        {
            Expression<Func<IQueryable<object>>> select = () => Queryable.Select(default(IQueryable<object>), default(Expression<Func<object, object>>));
            MethodInfo method = ((MethodCallExpression)select.Body).Method.GetGenericMethodDefinition();
            return method;
        }

        public IQueryable<TResult> To<TResult>(object parameters = null)
        {
            return this.To<TResult>(parameters, new string[0]);
        }

        public IQueryable<TResult> To<TResult>(object parameters = null, params string[] membersToExpand)
        {
            var paramValues = GetParameters(parameters);
            return this.To<TResult>(paramValues, membersToExpand);
        }

        private static IObjectDictionary GetParameters(object parameters)
        {
            return (parameters ?? new object()).GetType()
                .GetDeclaredProperties()
                .ToDictionary(pi => pi.Name, pi => pi.GetValue(parameters, null));
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters)
        {
            return this.To<TResult>(parameters, new string[0]);
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters, params string[] membersToExpand)
        {
            var members = this.GetMemberPaths(typeof(TResult), membersToExpand);
            return this.To<TResult>(parameters, members);
        }

        public IQueryable<TResult> To<TResult>(object parameters = null, params Expression<Func<TResult, object>>[] membersToExpand)
        {
            return this.To<TResult>(GetParameters(parameters), this.GetMemberPaths(membersToExpand));
        }

        private MemberPaths GetMemberPaths(Type type, string[] membersToExpand)
        {
            return membersToExpand.Select(m=>ReflectionHelper.GetMemberPath(type, m));
        }

        private MemberPaths GetMemberPaths<TResult>(Expression<Func<TResult, object>>[] membersToExpand)
        {
            return membersToExpand.Select(expr =>
            {
                var visitor = new MemberVisitor();
                visitor.Visit(expr);
                return visitor.MemberPath;
            });
        }

        public IQueryable<TResult> To<TResult>(IObjectDictionary parameters, params Expression<Func<TResult, object>>[] membersToExpand)
        {
            var members = this.GetMemberPaths(membersToExpand);
            return this.To<TResult>(parameters, members);
        }

        private IQueryable<TResult> To<TResult>(IObjectDictionary parameters, MemberPaths memberPathsToExpand)
        {
            var membersToExpand = memberPathsToExpand.SelectMany(m => m).Distinct().ToArray();

            var mapExpression = _builder.CreateMapExpression(_source.ElementType, typeof(TResult), parameters, membersToExpand);

            return _source.Provider.CreateQuery<TResult>(
                Expression.Call(
                    null,
                    QueryableSelectMethod.MakeGenericMethod(_source.ElementType, typeof(TResult)),
                    new[] { _source.Expression, Expression.Quote(mapExpression) }
                    )
                );
        }

        private class MemberVisitor : ExpressionVisitor
        {
            protected override Expression VisitLambda<T>(Expression<T> node)
            {
                var memberExpression = node.Body as MemberExpression;
                if(memberExpression != null)
                {
                    if(this.MemberPath != null)
                    {
                        throw new InvalidOperationException("There are more than one lambda member expressions.");
                    }
                    this.MemberPath = this.GetMemberPath(memberExpression);
                }
                return base.VisitLambda<T>(node);
            }

            private IEnumerable<MemberInfo> GetMemberPath(MemberExpression memberExpression)
            {
                var expression = memberExpression;
                while(expression != null)
                {
                    yield return expression.Member;
                    expression = expression.Expression as MemberExpression;
                }
            }

            public IEnumerable<MemberInfo> MemberPath { get; private set; }
        }
    }
}