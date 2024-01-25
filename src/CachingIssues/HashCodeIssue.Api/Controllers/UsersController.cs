using HashCodeIssue.Api.Domain.Entities;
using HashCodeIssue.Api.Persistence.Caching.Brokers;
using HashCodeIssue.Api.Persistence.DataContexts;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace HashCodeIssue.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController(AppDbContext appDbContext, ICacheBroker cacheBroker) : ControllerBase
{
    [HttpGet("a")]
    public async ValueTask<IActionResult> GetUsersA()
    {
        // Create query and cache key
        var usersQuery = appDbContext.Users.Skip(0).Take(10);
        var expressionComparer = ExpressionEqualityComparer.Instance;
        var cacheKey = expressionComparer.GetHashCode(usersQuery.Expression);
        
        // Get users
        var users = await cacheBroker.GetOrSetAsync<List<User>>(cacheKey.ToString(), () => usersQuery.ToListAsync());

        return Ok(users);
    }
    
    [HttpGet("b")]
    public async ValueTask<IActionResult> GetUsersB()
    {
        // Create query and cache key
        var usersQuery = appDbContext.Users.Skip(10).Take(10);
        var expressionComparer = ExpressionEqualityComparer.Instance;
        var cacheKey = expressionComparer.GetHashCode(usersQuery.Expression);
        
        // Get users
        var users = await cacheBroker.GetOrSetAsync<List<User>>(cacheKey.ToString(), () => usersQuery.ToListAsync());

        return Ok(users);
    }
}