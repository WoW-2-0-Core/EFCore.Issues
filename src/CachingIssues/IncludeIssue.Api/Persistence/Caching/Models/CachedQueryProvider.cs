using System.Linq.Expressions;
using IncludeIssue.Api.Domain.Extensions;
using Microsoft.EntityFrameworkCore.Query;

namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Represents query provider wrapper for caching
/// </summary>
/// <param name="queryProvider">The base query provider</param>
/// <param name="queryCacheBroker">The query cache resolver used for caching</param>
public class CachedQueryProvider<TSource, TQueryable>(
    CachedQueryable<TSource, TQueryable> cachedQueryable,
    IAsyncQueryProvider queryProvider,
    IExpressionCacheKeyResolver expressionCacheKeyResolver,
    IQueryCacheBroker queryCacheBroker
) : IAsyncQueryProvider where TQueryable : IQueryable<TSource>, IAsyncEnumerable<TSource>
{
    public IQueryable CreateQuery(Expression expression)
    {
        return new CachedQueryable<TSource, TQueryable>(cachedQueryable, expression);
    }

    public IQueryable<TElement> CreateQuery<TElement>(Expression expression)
    {
        // Use cached queryable if element type is same as source type
        if (typeof(TElement) == typeof(TSource))
            return (IQueryable<TElement>)new CachedQueryable<TSource, TQueryable>(cachedQueryable, expression);

        return queryProvider.CreateQuery<TElement>(expression);
    }

    public object? Execute(Expression expression)
    {
        return queryProvider.Execute(expression);
    }

    public TResult Execute<TResult>(Expression expression)
    {
        return queryProvider.Execute<TResult>(expression);
    }

    public TResult ExecuteAsync<TResult>(Expression expression, CancellationToken cancellationToken = default)
    {
        var resultType = typeof(TResult);
        var actualType = resultType;

        // Determine actual type
        var isTask = resultType.IsTask();
        if (isTask) actualType = resultType.GetGenericArgument()!;
        if (actualType.IsCollection()) actualType = resultType.GetGenericArgument();

        var test = queryProvider.ExecuteAsync<TResult>(expression, cancellationToken)!;
        
        // Get cache key
        var cacheKey = expressionCacheKeyResolver.GetCacheKey<TResult>(expression, actualType);

        // Get method of query cache resolver and invoke it
        var method = typeof(IQueryCacheBroker).GetMethod(
            resultType.IsTask() ? nameof(IQueryCacheBroker.GetOrSetAsync) : nameof(IQueryCacheBroker.GetOrSet)
        )!;
        var generic = method.MakeGenericMethod(resultType, actualType!);
        var result = (TResult)generic.Invoke(
            queryCacheBroker,
            [cacheKey, () => queryProvider.ExecuteAsync<TResult>(expression, cancellationToken)!]
        )!;

        return result;
    }
}