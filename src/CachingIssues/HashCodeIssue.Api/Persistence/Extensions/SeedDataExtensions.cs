using Bogus;
using HashCodeIssue.Api.Domain.Entities;
using HashCodeIssue.Api.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace HashCodeIssue.Api.Persistence.Extensions;

/// <summary>
/// Extension methods for initializing seed data in the application.
/// </summary>
public static class SeedDataExtensions
{
    /// <summary>
    /// Initializes seed data in the AppDbContext by checking for existing users and seeding them if necessary.
    /// </summary>
    /// <param name="serviceProvider">The service provider to resolve dependencies.</param>
    /// <returns>An asynchronous task representing the initialization process.</returns>
    public static async ValueTask InitializeSeedAsync(this IServiceProvider serviceProvider)
    {
        var scope = serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await appDbContext.Users.AnyAsync()) await appDbContext.SeedUsersAsync();
    }

    /// <summary>
    /// Seeds user data
    /// </summary>
    /// <param name="dbContext">The AppDbContext instance to seed data into.</param>
    /// <returns>An asynchronous task representing the seeding process.</returns>
    private static async ValueTask SeedUsersAsync(this AppDbContext dbContext)
    {
        var guestFaker = new Faker<User>()
            .RuleFor(user => user.FirstName, data => data.Name.FirstName())
            .RuleFor(user => user.LastName, data => data.Name.LastName())
            .RuleFor(user => user.EmailAddress, data => data.Person.Email);

        await dbContext.AddRangeAsync(guestFaker.Generate(100));
        await dbContext.SaveChangesAsync();
    }
}