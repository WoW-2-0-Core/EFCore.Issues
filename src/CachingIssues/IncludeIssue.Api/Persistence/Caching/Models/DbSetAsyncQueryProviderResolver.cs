using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.EntityFrameworkCore.Query;

namespace IncludeIssue.Api.Persistence.Caching.Models;

/// <summary>
/// Represents a resolver for async query provider.
/// </summary>
/// <param name="dbSet">An instance of <see cref="DbSet{TEntity}"/></param>
/// <typeparam name="TEntity">Entity type</typeparam>
public readonly struct DbSetAsyncQueryProviderResolver<TEntity>(DbSet<TEntity> dbSet) : IAsyncQueryProviderResolver where TEntity : class
{
    public IAsyncQueryProvider Get()
    {
        if (dbSet is not InternalDbSet<TEntity>)
            throw new ArgumentException("DbSet must be an InternalDbSet", nameof(dbSet));

        var entityQueryableMember = dbSet.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(field => field.Name == "_entityQueryable");
        var entityQueryableInstance = entityQueryableMember.GetValue(dbSet) ?? throw new NullReferenceException("Entity queryable is null");

        // Get async query provider
        var asyncQueryProviderMember = entityQueryableInstance.GetType()
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(field => field.Name == "_queryProvider");
        var asyncQueryProviderInstance = asyncQueryProviderMember.GetValue(entityQueryableInstance) ??
                                         throw new NullReferenceException("Async query provider is null");

        return asyncQueryProviderInstance as IAsyncQueryProvider ?? throw new NullReferenceException("Async query provider is null");
    }
}