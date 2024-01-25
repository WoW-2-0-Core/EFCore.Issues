using Bogus;
using IncludeIssue.Api.Domain.Entities;
using IncludeIssue.Api.Persistence.DataContexts;
using Microsoft.EntityFrameworkCore;

namespace IncludeIssue.Api.Persistence.Extensions;

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

        if (!await appDbContext.UserSettings.AnyAsync()) await appDbContext.SeedUserSettingsAsync();
    }

    /// <summary>
    /// Seeds user data
    /// </summary>
    /// <param name="dbContext">The AppDbContext instance to seed data into.</param>
    /// <returns>An asynchronous task representing the seeding process.</returns>
    private static async ValueTask SeedUsersAsync(this AppDbContext dbContext)
    {
        var guestFaker = new Faker<User>().RuleFor(user => user.FirstName, data => data.Name.FirstName())
            .RuleFor(user => user.LastName, data => data.Name.LastName())
            .RuleFor(user => user.EmailAddress, data => data.Person.Email);

        await dbContext.AddRangeAsync(guestFaker.Generate(100));
        await dbContext.SaveChangesAsync();
    }
    
    /// <summary>
    /// Seeds user data
    /// </summary>
    /// <param name="dbContext">The AppDbContext instance to seed data into.</param>
    /// <returns>An asynchronous task representing the seeding process.</returns>
    private static async ValueTask SeedUserSettingsAsync(this AppDbContext dbContext)
    {
        var users = new Stack<User>(await dbContext.Users.ToListAsync());
        
        var guestFaker = new Faker<UserSettings>()
            .RuleFor(user => user.UserId, () => users.Pop().Id)
            .RuleFor(user => user.Language, src => src.PickRandom("English", "Spanish"))
            .RuleFor(user => user.Theme, src => src.PickRandom("Dark", "Light"));

        await dbContext.AddRangeAsync(guestFaker.Generate(100));
        await dbContext.SaveChangesAsync();
    }
}