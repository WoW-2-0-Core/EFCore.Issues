using IncludeIssue.Api.Infrastructure.Common.Caching.Brokers;
using IncludeIssue.Api.Persistence.Caching.Brokers;
using IncludeIssue.Api.Persistence.DataContexts;
using IncludeIssue.Api.Persistence.Extensions;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Register in memory cache
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSingleton<ICacheBroker, DistributedCacheBroker>();

// Register data contexts
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("EfCoreIssues.HashCodeIssue"));

// Register exposers
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// Register dev tools
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// initialize seed data
await app.Services.InitializeSeedAsync();

// use exposers
app.MapControllers();

// use dev tools
app.UseSwagger();
app.UseSwaggerUI();

app.Run();