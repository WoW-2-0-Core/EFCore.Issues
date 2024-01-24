namespace UpdateIssue.Api.Domain.Common;

public interface IEntity
{
    /// <summary>
    /// Gets or sets the unique identifier of the entity.
    /// </summary>
    /// <value>
    /// The unique identifier of the entity.
    /// </value>
    public Guid Id { get; set; }
}