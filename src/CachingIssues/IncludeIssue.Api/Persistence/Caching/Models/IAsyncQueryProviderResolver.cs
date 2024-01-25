using Microsoft.EntityFrameworkCore.Query;

namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Defines a resolver for async query provider.
/// </summary>
public interface IAsyncQueryProviderResolver
{
    /// <summary>
    /// Gets async query provider.
    /// </summary>
    /// <returns><see cref="IAsyncQueryProvider"/> instance</returns>
    IAsyncQueryProvider Get();
}