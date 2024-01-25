using IncludeIssue.Api.Domain.Entities;
using IncludeIssue.Api.Persistence.Caching.Brokers;
using IncludeIssue.Api.Persistence.Caching.Models;
using IncludeIssue.Api.Persistence.DataContexts;
using IncludeIssue.Api.Persistence.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IncludeIssue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext appDbContext, ICacheBroker cacheBroker) : ControllerBase
{
    [HttpGet("a")]
    public async ValueTask<IActionResult> GetUsersA()
    {
        // Create cacheable query
        var initialQuery = appDbContext.Users
            .AddCaching(
            new DbSetAsyncQueryProviderResolver<User>(appDbContext.Users),
            new EfCoreExpressionCacheKeyResolver(),
            cacheBroker.GetCacheResolver()
        );

        var users = await initialQuery.Include(user => user.UserSettings).ToListAsync();

        // Get users
        return Ok(users);
    }
    
    [HttpGet("b")]
    public async ValueTask<IActionResult> GetUsersB()
    {
        // Create cacheable query
        var initialQuery = appDbContext.Users.AsQueryable();

        var users = await initialQuery.Include(user => user.UserSettings).ToListAsync();

        // Get users
        return Ok(users);
    }
}