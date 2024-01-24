using Bogus;
using Microsoft.EntityFrameworkCore;
using UpdateIssue.Api.Domain.Entities;
using UpdateIssue.Api.Persistence.DataContexts;

namespace UpdateIssue.Api.Persistence.SeedData;

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
        using var scope = serviceProvider.CreateScope();
        var appDbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        if (!await appDbContext.GuestFeedbacks.AnyAsync())
            await appDbContext.SeedGuestFeedbacksAsync();
    }
    
    /// <summary>
    /// Seeds Guest Feedbacks data into the AppDbContext using Bogus library.
    /// </summary>
    /// <param name="dbContext"></param>
    private static async ValueTask SeedGuestFeedbacksAsync(this AppDbContext dbContext)
    {
        var feedbackFaker = new Faker<GuestFeedback>()
            .RuleFor(feedback => feedback.Rating, GenerateFakeRating)
            .RuleFor(feedback => feedback.Comment, data => data.Lorem.Paragraph());
        
        await dbContext.GuestFeedbacks.AddRangeAsync(feedbackFaker.Generate(100));
        await dbContext.SaveChangesAsync();
    }

    private static Rating GenerateFakeRating()
    {
        var ratingsFaker = new Faker<Rating>()
            .RuleFor(rating => rating.Accuracy, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.Cleanliness, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.Communication, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.CheckIn, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.Location, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.Value, data => data.Random.Byte(1, 5))
            .RuleFor(rating => rating.OverallRating, (data, rating) => 
                (rating.Accuracy + rating.Value + rating.Cleanliness + 
                 rating.Communication + rating.Location + rating.CheckIn) / 6);

        return ratingsFaker.Generate(1)[0];
    }
}