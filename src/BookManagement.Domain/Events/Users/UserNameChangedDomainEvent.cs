namespace BookManagement.Domain.Events.Users;

/// <summary> 
/// Event raised when a user's name is changed.
/// <param name="Id">The unique identifier for the event.</param>
/// <param name="UserId">The unique identifier of the user whose name was changed.</param>
/// </summary>
public sealed record UserNameChangedDomainEvent(
    Guid Id, 
    Guid UserId) : DomainEvent(Id);