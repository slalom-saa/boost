using System.Collections.Concurrent;
using System.Linq.Expressions;

namespace Slalom.Boost.AutoMapper.QueryableExtensions
{
    public interface IExpressionBinder
    {
        bool IsMatch(PropertyMap propertyMap, TypeMap propertyTypeMap, ExpressionResolutionResult result);

        MemberAssignment Build(IConfigurationProvider configuration, PropertyMap propertyMap, TypeMap propertyTypeMap, ExpressionRequest request, ExpressionResolutionResult result, ConcurrentDictionary<ExpressionRequest, int> typePairCount);
    }
}