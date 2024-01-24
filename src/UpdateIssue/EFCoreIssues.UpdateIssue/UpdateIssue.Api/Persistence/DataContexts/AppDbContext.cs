using Microsoft.EntityFrameworkCore;
using UpdateIssue.Api.Domain.Entities;

namespace UpdateIssue.Api.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<GuestFeedback> GuestFeedbacks => Set<GuestFeedback>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Guest feedback configuration
        modelBuilder.Entity<GuestFeedback>().OwnsOne(feedback => feedback.Rating);
        modelBuilder.Entity<GuestFeedback>().Navigation(feedback => feedback.Rating).IsRequired();
    }
}