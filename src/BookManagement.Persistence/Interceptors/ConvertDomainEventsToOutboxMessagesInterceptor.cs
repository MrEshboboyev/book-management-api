using BookManagement.Domain.Primitives;
using BookManagement.Persistence.Outbox;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Newtonsoft.Json;

namespace BookManagement.Persistence.Interceptors;

/// <summary>
/// Intercepts save changes operations to convert domain events into outbox messages.
/// </summary>
public sealed class ConvertDomainEventsToOutboxMessagesInterceptor
    : SaveChangesInterceptor
{
    /// <summary>
    /// Overrides the async saving changes method to convert domain events into outbox messages.
    /// </summary>
    /// <param name="eventData">The event data containing information about the DbContext.</param> 
    /// <param name="result">The result of the interception.</param> 
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A ValueTask representing the asynchronous save operation,
    /// including the interception result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        #region Get DbContext
        
        // Get the current DbContext
        var dbContext = eventData.Context;
        if (dbContext is null)
        {
            return base.SavingChangesAsync(
                eventData,
                result,
                cancellationToken);
        }
        
        #endregion

        #region Converting
        
        // Convert domain events to outbox messages
        var outboxMessages = dbContext.ChangeTracker
            .Entries<AggregateRoot>() // Get entries of AggregateRoot type 
            .Select(x => x.Entity) // Select the AggregateRoot entities
            .SelectMany(aggregateRoot =>
            {
                var domainEvents = aggregateRoot.GetDomainEvents(); // Get domain events
                aggregateRoot.ClearDomainEvents(); // Clear domain events from aggregate
                return domainEvents; // Return domain events
            })
            .Select(domainEvent => new OutboxMessage // Create new OutboxMessage for adding to dbContext
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().Name,
                Content = JsonConvert.SerializeObject(
                    domainEvent,
                    new JsonSerializerSettings
                    {
                        TypeNameHandling = TypeNameHandling.All,
                    })
            })
            .ToList();
        
        #endregion
        
        #region Add and Update Database

        // Add outbox messages to the DbContext
        dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

        // Continue with the base saving changes operation
        return base.SavingChangesAsync(eventData, result, cancellationToken);
        
        #endregion
    }
}