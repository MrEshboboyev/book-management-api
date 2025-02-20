using BookManagement.Domain.Events;

namespace BookManagement.Domain.Events.Books;

public sealed record BookUpdatedDomainEvent(
    Guid Id,
    Guid BookId) : DomainEvent(Id);