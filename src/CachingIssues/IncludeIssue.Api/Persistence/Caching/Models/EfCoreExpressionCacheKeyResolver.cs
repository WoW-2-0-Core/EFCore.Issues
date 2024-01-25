using System.Linq.Expressions;
using IncludeIssue.Api.Domain.Constants;
using IncludeIssue.Api.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Represents a resolver for cache key.
/// </summary>
public readonly struct EfCoreExpressionCacheKeyResolver : IExpressionCacheKeyResolver
{
    public string GetCacheKey<T>(Expression expression, Type? actualType = default)
    {
        var resultType = typeof(T);
        var isCollection = resultType.IsCollection();

        if (actualType is null)
        {
            actualType = resultType;

            // Determine actual type
            var isTask = resultType.IsTask();
            if (isTask) actualType = resultType.GetGenericArgument()!;
            if (actualType.IsCollection()) actualType = resultType.GetGenericArgument();
        }
        
        var instance = ExpressionEqualityComparer.Instance;
        var hashCode = instance.GetHashCode(expression);

        var postFix = isCollection
            ? InfrastructureConstants.CachingConstants.MultipleEntryPrefix
            : InfrastructureConstants.CachingConstants.SingleEntryPrefix;
        return $"{actualType!.Name}_{postFix}_{hashCode}";
    }
}