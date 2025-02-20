using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Events.Books;

namespace BookManagement.Application.Books.Events;

internal sealed class BookCreatedDomainEventHandler : IDomainEventHandler<BookCreatedDomainEvent>
{
    public Task Handle(BookCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        Console.WriteLine($"Book added: {notification.Title}");
        return Task.CompletedTask;
    }
}
