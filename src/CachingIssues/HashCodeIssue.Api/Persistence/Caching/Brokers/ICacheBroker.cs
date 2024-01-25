namespace HashCodeIssue.Api.Persistence.Caching.Brokers;

public interface ICacheBroker
{
    ValueTask<T?> GetOrSetAsync<T>(
        string key,
        Func<Task<T>> valueFactory,
        CancellationToken cancellationToken = default
    );
}