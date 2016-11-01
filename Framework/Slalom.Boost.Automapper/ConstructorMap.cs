using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Slalom.Boost.AutoMapper.Internal;
using Slalom.Boost.AutoMapper.QueryableExtensions;
using Slalom.Boost.AutoMapper.QueryableExtensions.Impl;

namespace Slalom.Boost.AutoMapper
{
    public class ConstructorMap
    {
        private static readonly DelegateFactory DelegateFactory = new DelegateFactory();
        private readonly Lazy<LateBoundParamsCtor> _runtimeCtor;
        public ConstructorInfo Ctor { get; private set; }
        public IEnumerable<ConstructorParameterMap> CtorParams { get; }

        public ConstructorMap(ConstructorInfo ctor, IEnumerable<ConstructorParameterMap> ctorParams)
        {
            this.Ctor = ctor;
            this.CtorParams = ctorParams;

            _runtimeCtor = new Lazy<LateBoundParamsCtor>(() => DelegateFactory.CreateCtor(ctor, this.CtorParams));
        }

        private static readonly IExpressionResultConverter[] ExpressionResultConverters =
        {
            new MemberGetterExpressionResultConverter(),
            new MemberResolverExpressionResultConverter(),
            new NullSubstitutionExpressionResultConverter()
        };

        public Expression NewExpression(Expression instanceParameter)
        {
            var parameters = this.CtorParams.Select(map =>
            {
                var result = new ExpressionResolutionResult(instanceParameter, this.Ctor.DeclaringType);
                foreach (var resolver in map.SourceResolvers)
                {
                    var matchingExpressionConverter =
                        ExpressionResultConverters.FirstOrDefault(c => c.CanGetExpressionResolutionResult(result, resolver));
                    if (matchingExpressionConverter == null)
                        throw new Exception("Can't resolve this to Queryable Expression");
                    result = matchingExpressionConverter.GetExpressionResolutionResult(result, map, resolver);
                }
                return result;
            });
            return Expression.New(this.Ctor, parameters.Select(p => p.ResolutionExpression));
        }

        public object ResolveValue(ResolutionContext context)
        {
            var ctorArgs = new List<object>();

            foreach (var map in this.CtorParams)
            {
                var result = map.ResolveValue(context);

                var sourceType = result.Type;
                var destinationType = map.Parameter.ParameterType;

                var typeMap = context.ConfigurationProvider.ResolveTypeMap(result, destinationType);

                Type targetSourceType = typeMap != null ? typeMap.SourceType : sourceType;

                var newContext = context.CreateTypeContext(typeMap, result.Value, null, targetSourceType,
                    destinationType);

                if (typeMap == null && map.Parameter.IsOptional)
                {
                    object value = map.Parameter.DefaultValue;
                    ctorArgs.Add(value);
                }
                else
                {
                    var value = context.Engine.Map(newContext);
                    ctorArgs.Add(value);
                }
            }

            return _runtimeCtor.Value(ctorArgs.ToArray());
        }
    }
}