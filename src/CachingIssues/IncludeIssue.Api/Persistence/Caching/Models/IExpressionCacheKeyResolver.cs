using System.Linq.Expressions;

namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Defines a resolver for cache key.
/// </summary>
public interface IExpressionCacheKeyResolver
{
    /// <summary>
    /// Computes cache key for given expression and actual type
    /// </summary>
    /// <param name="expression">The expression to compute cache key for</param>
    /// <param name="actualType">The expression actual return type</param>
    /// <typeparam name="TResult">Result type of expression</typeparam>
    /// <returns>Computed cache key</returns>
    string GetCacheKey<TResult>(Expression expression, Type? actualType = default);
}