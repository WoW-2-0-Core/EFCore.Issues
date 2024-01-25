namespace IncludeIssue.Api.Domain.Collections;

/// <summary>
/// Represents async enumerable collection
/// </summary>
/// <param name="source"></param>
/// <typeparam name="TElement"></typeparam>
public class AsyncEnumerable<TElement>(IEnumerable<TElement> source) : IAsyncEnumerable<TElement>
{
    public async IAsyncEnumerator<TElement> GetAsyncEnumerator(CancellationToken cancellationToken = default)
    {
        foreach (var item in source)
            yield return await Task.FromResult(item);
    }
}