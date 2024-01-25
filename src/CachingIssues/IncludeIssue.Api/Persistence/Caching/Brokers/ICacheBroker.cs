using IncludeIssue.Api.Persistence.Caching.Models;

namespace IncludeIssue.Api.Persistence.Caching.Brokers;

/// <summary>
/// Defines cache broker functionality
/// </summary>
public interface ICacheBroker
{
    /// <summary>
    /// Retrieves the cache entry with given key
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve.</typeparam>
    /// <param name="key">The key of the value to retrieve.</param>
    /// <returns>Cache entry if exists, otherwise null</returns>
    ValueTask<T?> GetAsync<T>(string key);

    /// <summary>
    /// Retrieves the cache entry from either the cache or the valueFactory function.
    /// </summary>
    /// <typeparam name="T">The type of the value to retrieve or generate.</typeparam>
    /// <param name="key">The key used to identify the value in the cache.</param>
    /// <param name="valueFactory">The function used to get the value.</param>
    /// <param name="entryOptions">The cache entry options for the value. Optional.</param>
    /// <returns>
    /// The value associated with the specified key if it exists,
    /// or the generated value from the valueFactory function otherwise.
    /// </returns>
    ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = default);

    /// <summary>
    /// Sets the value of an item in the cache.
    /// </summary>
    /// <param name="key">The cache key for entry.</param>
    /// <param name="value">Cache entry to be stored.</param>
    /// <param name="entryOptions">The cache entry options.</param>
    /// <param name="cancellationToken">Cancellation token</param>
    /// <typeparam name="T">The type of the value to be stored.</typeparam>
    ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default, CancellationToken cancellationToken = default);

    /// <summary>
    /// Creates cache resolver with given entry options
    /// </summary>
    /// <param name="entryOptions">The cache entry options.</param>
    /// <returns>Created cache resolver</returns>
    IQueryCacheBroker GetCacheResolver(CacheEntryOptions? entryOptions = default);
}