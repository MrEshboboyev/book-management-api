using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Primitives;
using BookManagement.Persistence;
using BookManagement.Persistence.Outbox;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Idempotence;

/// <summary> 
/// Handles domain events in an idempotent manner, ensuring that an event is processed only once. 
/// </summary> 
/// <typeparam name="TDomainEvent">The type of the domain event.</typeparam>
public class IdempotentDomainEventHandler<TDomainEvent>(
    INotificationHandler<TDomainEvent> decorated,
    ApplicationDbContext dbContext)
    : IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    /// <summary> 
    /// Handles the domain event. 
    /// </summary> 
    /// <param name="notification">The domain event notification.</param> 
    /// <param name="cancellationToken">Cancellation token.</param>
    public async Task Handle(TDomainEvent notification, CancellationToken cancellationToken)
    {
        #region Get Consumer
        
        var consumer = decorated.GetType().Name;
        
        #endregion

        #region Check Domain Event processed
        
        // Check if the domain event has already been processed by querying the database.
        if (await dbContext.Set<OutboxMessageConsumer>()
                .AnyAsync(o => o.Id == notification.Id &&
                               o.Name == consumer, cancellationToken: cancellationToken))
        {
            return; // Event already processed, exit method.
        }
        
        #endregion

        #region Handle Domain Event
        
        // Handle the domain event.
        await decorated.Handle(notification, cancellationToken);
        
        #endregion

        #region Add and Update database
        
        // Record the processing of the event.
        dbContext.Set<OutboxMessageConsumer>()
            .Add(new OutboxMessageConsumer
            {
                Id = notification.Id,
                Name = consumer
            });

        // Save changes to the database.
        await dbContext.SaveChangesAsync(cancellationToken);
        
        #endregion
    }
}