using IncludeIssue.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IncludeIssue.Api.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    public DbSet<UserSettings> UserSettings => Set<UserSettings>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(
            builder =>
            {
                builder.Property(user => user.FirstName).HasMaxLength(256).IsRequired();
                builder.Property(user => user.LastName).HasMaxLength(256).IsRequired();
                builder.Property(user => user.EmailAddress).HasMaxLength(256).IsRequired();

                builder.HasOne(user => user.UserSettings).WithOne().HasForeignKey<UserSettings>(userSettings => userSettings.UserId);
            }
        );
        
        modelBuilder.Entity<UserSettings>(
            builder =>
            {
                builder.HasKey(userSettings => userSettings.UserId);
                builder.Property(userSettings => userSettings.Language).HasMaxLength(256).IsRequired();
                builder.Property(userSettings => userSettings.Theme).HasMaxLength(256).IsRequired();
            }
        );
    }
}