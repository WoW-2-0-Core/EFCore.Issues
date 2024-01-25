using HashCodeIssue.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HashCodeIssue.Api.Persistence.DataContexts;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
}