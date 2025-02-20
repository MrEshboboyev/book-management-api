using BookManagement.Domain.Events;

namespace BookManagement.Domain.Events.Books;

public sealed record BookDeletedDomainEvent(
    Guid Id, 
    Guid BookId) : DomainEvent(Id);