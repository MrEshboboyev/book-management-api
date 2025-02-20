namespace BookManagement.Domain.Events.Books;

public sealed record BookViewedDomainEvent(
    Guid Id, 
    Guid BookId, 
    int ViewsCount) : DomainEvent(Id);
