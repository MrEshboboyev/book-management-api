namespace BookManagement.Domain.Events.Books;

public sealed record BookCreatedDomainEvent(
    Guid Id, 
    Guid BookId) : DomainEvent(Id);