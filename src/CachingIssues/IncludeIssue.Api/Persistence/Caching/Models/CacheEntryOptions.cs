namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Represents cache entry options
/// </summary>
public struct CacheEntryOptions
{
    /// <summary>
    /// Gets or sets the absolute expiration date for the cache entry.
    /// </summary>
    public TimeSpan? AbsoluteExpirationRelativeToNow { get; set; }

    /// <summary>
    /// Gets or sets the sliding expiration time for the cache entry.
    /// </summary>
    public TimeSpan? SlidingExpiration { get; set; }
}