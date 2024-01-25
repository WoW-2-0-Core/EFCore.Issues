using IncludeIssue.Api.Domain.Collections;
using IncludeIssue.Api.Domain.Extensions;
using IncludeIssue.Api.Persistence.Caching.Brokers;
using IncludeIssue.Api.Persistence.Caching.Models;

namespace IncludeIssue.Api.Infrastructure.Common.Caching.Brokers;

/// <summary>
/// Defines query cache resolver with type fixes
/// </summary>
public readonly struct QueryCacheBroker(CacheEntryOptions cacheEntryOptions, ICacheBroker cacheBroker) : IQueryCacheBroker
{
    // TODO: Optimize this
    public async Task<TResult> GetOrSetAsync<TResult, TActual>(string key, Func<Task<TResult>> valueFactory) where TResult : class
    {
        var resultType = typeof(TResult);

        // If single value, just return it
        if (!resultType.IsCollection())
            return (await cacheBroker.GetOrSetAsync(key, valueFactory))!;

        // If collection, resolve type
        if (resultType == typeof(IAsyncEnumerable<TActual>))
        {
            var value = await cacheBroker.GetAsync<List<TActual>>(key);

            if (value is not null)
                return (new AsyncEnumerable<TActual>(value) as TResult)!;

            var result = await valueFactory();
            await cacheBroker.SetAsync(key, result, cacheEntryOptions);

            return result;
        }

        if (resultType == typeof(IEnumerable<TActual>))
        {
            var value = await cacheBroker.GetAsync<List<TActual>>(key);

            if (value is not null)
                return (value.AsEnumerable() as TResult)!;

            var result = await valueFactory();
            await cacheBroker.SetAsync(key, result, cacheEntryOptions);

            return result!;
        }

        throw new NotSupportedException("Not supported type");
    }

    public TResult GetOrSet<TResult, TActual>(string key, Func<TResult> valueFactory) where TResult : class
    {
        var getOrSetTask = GetOrSetAsync<TResult, TActual>(key, () => Task.FromResult(valueFactory()));
        getOrSetTask.Wait();

        return getOrSetTask.Result;
    }
}