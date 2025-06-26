using BookManagement.Domain.Events;
using BookManagement.Domain.Identity.Books;

namespace BookManagement.Domain.Events.Books;

public sealed record BookDeletedDomainEvent(
    Guid Id, 
    BookId BookId) : DomainEvent(Id);
