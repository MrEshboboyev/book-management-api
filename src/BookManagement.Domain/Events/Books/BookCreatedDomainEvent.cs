using BookManagement.Domain.Identity.Books;

namespace BookManagement.Domain.Events.Books;

public sealed record BookCreatedDomainEvent(
    Guid Id, 
    BookId BookId, 
    string Title) : DomainEvent(Id);
