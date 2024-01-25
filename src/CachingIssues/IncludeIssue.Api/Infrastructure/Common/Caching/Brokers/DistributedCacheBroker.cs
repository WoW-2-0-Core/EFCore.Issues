using Force.DeepCloner;
using IncludeIssue.Api.Persistence.Caching.Brokers;
using IncludeIssue.Api.Persistence.Caching.Models;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace IncludeIssue.Api.Infrastructure.Common.Caching.Brokers;

public class DistributedCacheBroker(
    IDistributedCache distributedCache
) : ICacheBroker
{
    private readonly CacheEntryOptions _entryOptions = new()
    {
        AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(600),
        SlidingExpiration = TimeSpan.FromSeconds(300)
    };

    public async ValueTask<T?> GetAsync<T>(string key)
    {
        var value = await distributedCache.GetStringAsync(key);

        return value is not null ? JsonConvert.DeserializeObject<T>(value) : default;
    }

    public async ValueTask<T?> GetOrSetAsync<T>(string key, Func<Task<T>> valueFactory, CacheEntryOptions? entryOptions = default)
    {
        var cachedValue = await distributedCache.GetStringAsync(key);
        if (cachedValue is not null) return JsonConvert.DeserializeObject<T>(cachedValue);

        var value = await valueFactory();
        await SetAsync(key, await valueFactory());

        return value;
    }


    public async ValueTask SetAsync<T>(string key, T value, CacheEntryOptions? entryOptions = default, CancellationToken cancellationToken = default)
    {
        await distributedCache.SetStringAsync(
            key,
            JsonConvert.SerializeObject(value),
            MapCacheEntryOptions(GetCacheEntryOptions(entryOptions)),
            cancellationToken
        );
    }

    public IQueryCacheBroker GetCacheResolver(CacheEntryOptions? entryOptions = default)
    {
        return new QueryCacheBroker(GetCacheEntryOptions(entryOptions), this);
    }
    
    private CacheEntryOptions GetCacheEntryOptions(CacheEntryOptions? entryOptions)
    {
        if (!entryOptions.HasValue ||
            (!entryOptions.Value.AbsoluteExpirationRelativeToNow.HasValue && !entryOptions.Value.SlidingExpiration.HasValue))
            return _entryOptions;

        var currentEntryOptions = _entryOptions.DeepClone();

        currentEntryOptions.AbsoluteExpirationRelativeToNow = entryOptions.Value.AbsoluteExpirationRelativeToNow;
        currentEntryOptions.SlidingExpiration = entryOptions.Value.SlidingExpiration;

        return currentEntryOptions;
    }

    private DistributedCacheEntryOptions MapCacheEntryOptions(CacheEntryOptions entryOptions)
    {
        return new DistributedCacheEntryOptions
        {
            AbsoluteExpirationRelativeToNow = entryOptions.AbsoluteExpirationRelativeToNow,
            SlidingExpiration = entryOptions.SlidingExpiration
        };
    }
}