using IncludeIssue.Api.Persistence.Caching.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace IncludeIssue.Api.Persistence.Extensions;

/// <summary>
/// Provides extension methods for EF Core functionality
/// </summary>
public static class EfCoreExtensions
{
    /// <summary>
    /// Adds caching to <see cref="DbSet{TEntity}"/> instance
    /// </summary>
    /// <param name="dbSet">Instance of <see cref="DbSet{TEntity}"/> to add caching to</param>
    /// <param name="asyncQueryProviderResolver">Instance async query provider resolver</param>
    /// <param name="expressionCacheKeyResolver">Cache key resolver for expressions</param>
    /// <param name="queryCacheBroker">Cache resolver for queries</param>
    /// <typeparam name="TEntity">Type of entity</typeparam>
    /// <returns>Queryable source that includes caching</returns>
    /// <exception cref="ArgumentException">If given db set is not type of <see cref="InternalDbSet{TEntity}"/></exception>
    public static IQueryable<TEntity> AddCaching<TEntity>(
        this DbSet<TEntity> dbSet,
        IAsyncQueryProviderResolver asyncQueryProviderResolver,
        IExpressionCacheKeyResolver expressionCacheKeyResolver,
        IQueryCacheBroker queryCacheBroker
    ) where TEntity : class
    {
        if (dbSet is not InternalDbSet<TEntity> internalDbSet)
            throw new ArgumentException("DbSet must be an InternalDbSet", nameof(dbSet));

        return new CachedQueryable<TEntity, InternalDbSet<TEntity>>(
            internalDbSet,
            asyncQueryProviderResolver,
            expressionCacheKeyResolver,
            queryCacheBroker
        );
    }
}