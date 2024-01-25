namespace IncludeIssue.Api.Domain.Entities;

public class UserSettings
{
    public Guid UserId { get; set; }

    public string Language { get; set; } = default!;

    public string Theme { get; set; } = default!;
}