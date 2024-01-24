using Microsoft.EntityFrameworkCore;
using UpdateIssue.Api.Persistence.DataContexts;
using UpdateIssue.Api.Persistence.Interceptors;
using UpdateIssue.Api.Persistence.SeedData;

var builder = WebApplication.CreateBuilder(args);

// register interceptors
builder.Services.AddScoped<UpdateSoftDeletionInterceptor>();

// register data contexts
builder.Services.AddDbContext<AppDbContext>((provider, options) =>
{
    options
        .UseNpgsql(builder.Configuration.GetConnectionString("DbConnectionString"))
        .AddInterceptors(provider.GetRequiredService<UpdateSoftDeletionInterceptor>());
});

// register exposers
builder.Services.AddRouting(options => options.LowercaseUrls = true);
builder.Services.AddControllers();

// register dev tools
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