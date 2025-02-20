namespace BookManagement.Persistence.Outbox;

/// <summary>
/// Represents a message containing a domain event to be stored in the outbox table.
/// </summary>
public sealed class OutboxMessage
{
    /// <summary>
    /// Gets or sets the unique identifier for the outbox message.
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Gets or sets the type of the domain event.
    /// </summary>
    public string Type { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the content of the domain event serialized as a string.
    /// </summary>
    public string Content { get; set; }

    /// <summary>
    /// Gets or sets the UTC time when the domain event occurred.
    /// </summary>
    public DateTime OccurredOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the UTC time when the outbox message was processed.
    /// </summary>
    public DateTime? ProcessedOnUtc { get; set; }

    /// <summary>
    /// Gets or sets the error message if processing the outbox message failed.
    /// </summary>
    public string Error { get; set; }
}
