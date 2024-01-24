using UpdateIssue.Api.Domain.Common;

namespace UpdateIssue.Api.Domain.Entities;

/// <summary>
/// Represents a guest's feedback entity, including individual aspect ratings and an overall rating.
/// </summary>
public class GuestFeedback : SoftDeletedEntity
{
    /// <summary>
    /// Gets or sets the guest's comment regarding the experience.
    /// </summary>
    public string Comment { get; set; } = default!;

    /// <summary>
    /// Gets or sets the unique identifier of the listing associated with the guest feedback.
    /// </summary>
    public Guid ListingId { get; set; }

    /// <summary>
    /// Gets or sets a rating given by the guest.
    /// </summary>
    public Rating Rating { get; set; }
}