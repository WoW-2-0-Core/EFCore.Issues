using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using UpdateIssue.Api.Domain.Common;

namespace UpdateIssue.Api.Persistence.Interceptors;

/// <summary>
/// Represents a custom interceptor that automatically handles
/// soft deletion audit for entities before saving changes to the database.
/// </summary>
public class UpdateSoftDeletionInterceptor() : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new CancellationToken())
    {
        var softDeletedEntries = 
            eventData.Context!.ChangeTracker.Entries<ISoftDeletedEntity>().ToList();
        
        // Set IsDeleted and DeletedTime properties for deleted entities implementing ISoftDeletedEntity interface
        softDeletedEntries.ForEach(entry =>
        {
            if (entry.State != EntityState.Deleted) return;
            
            entry.State = EntityState.Modified;

            entry.Property(nameof(ISoftDeletedEntity.IsDeleted)).CurrentValue = true;
            entry.Property(nameof(ISoftDeletedEntity.DeletedTime)).CurrentValue = DateTimeOffset.UtcNow;
        });
        
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}