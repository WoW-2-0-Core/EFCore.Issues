namespace IncludeIssue.Api.Domain.Entities;

/// <summary>
/// Represents a storage file.
/// </summary>
public class User
{
    public Guid Id { get; set; }
    
    /// <summary>
    /// Gets or sets the first name.
    /// </summary>
    public string FirstName { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the last name
    /// </summary>
    public string LastName { get; set; } = default!;
    
    /// <summary>
    /// Gets or sets the file name.
    /// </summary>
    public string EmailAddress { get; set; } = default!;
    
    public UserSettings? UserSettings { get; set; }
}