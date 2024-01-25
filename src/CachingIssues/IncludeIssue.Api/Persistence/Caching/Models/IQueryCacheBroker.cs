namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Defines query cache resolver with type fixes
/// </summary>
public interface IQueryCacheBroker
{
    /// <summary>
    /// Retrieves the cache entry with given key with resolved actual type
    /// </summary>
    /// <typeparam name="TResult">The type of the query result.</typeparam>
    /// <typeparam name="TActual">The actual root type of query.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="valueFactory">The function used to get the value.</param>
    /// <returns>Value in resolved type that is convertible to TResult</returns>
    Task<TResult> GetOrSetAsync<TResult, TActual>(string key, Func<Task<TResult>> valueFactory) where TResult : class;

    /// <summary>
    /// Retrieves the cache entry with given key with resolved actual type
    /// </summary>
    /// <typeparam name="TResult">The type of the query result.</typeparam>
    /// <typeparam name="TActual">The actual root type of query.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <param name="valueFactory">The function used to get the value.</param>
    /// <returns>Value in resolved type that is convertible to TResult</returns>
    TResult GetOrSet<TResult, TActual>(string key, Func<TResult> valueFactory) where TResult : class;
}