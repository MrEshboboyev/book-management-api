namespace BookManagement.Persistence.Outbox;

/// <summary> 
/// Represents a record in the database that tracks processed domain events to ensure idempotence. 
/// </summary>
public sealed class OutboxMessageConsumer
{
    /// <summary> 
    /// Gets or sets the unique identifier of the domain event. 
    /// </summary>
    public Guid Id { get; set; }

    /// <summary> 
    /// Gets or sets the name of the event handler that processed the event. 
    /// </summary>
    public string Name { get; set; }
}