using BookManagement.Domain.Identity.Books;

namespace BookManagement.Domain.Events.Books;

public sealed record BookViewedDomainEvent(
    Guid Id, 
    BookId BookId, 
    int ViewsCount) : DomainEvent(Id);
