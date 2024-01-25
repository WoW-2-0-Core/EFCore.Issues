using HashCodeIssue.Api.Persistence.Caching.Brokers;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace HashCodeIssue.Api.Infrastructure.Common.Caching.Brokers;

/// <summary>
/// Provides functionality of Redis cache broker for distributed caching
/// </summary>
public class DistributedCacheBroker(
    IDistributedCache distributedCache
) : ICacheBroker
{
    private readonly DistributedCacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600),
        SlidingExpiration = TimeSpan.FromSeconds(300)
    };

    public async ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CancellationToken cancellationToken = default)
    {
        var cachedValue = await distributedCache.GetStringAsync(key, token: cancellationToken);
        if (cachedValue is not null) return JsonConvert.DeserializeObject<T>(cachedValue);

        var value = await valueFactory();
        await SetAsync(key, await valueFactory(), cancellationToken);

        return value;
    }

    private async ValueTask SetAsync<T>(string key, T value, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(value),
            _entryOptions,
            cancellationToken
        );
    }
}