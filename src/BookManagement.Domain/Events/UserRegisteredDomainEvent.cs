namespace BookManagement.Domain.Events;

/// <summary> 
/// Event raised when a user is registered.
/// <param name="Id">The unique identifier for the event.</param>
/// <param name="UserId">The unique identifier of the registered user.</param>
/// </summary>
public sealed record UserRegisteredDomainEvent(
    Guid Id,
    Guid UserId) : DomainEvent(Id);