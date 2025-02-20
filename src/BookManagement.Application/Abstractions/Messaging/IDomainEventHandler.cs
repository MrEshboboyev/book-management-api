using BookManagement.Domain.Primitives;
using MediatR;

namespace BookManagement.Application.Abstractions.Messaging;

/// <summary> 
/// Handles domain events. 
/// Domain event handlers contain the logic to process events that occur within the domain. 
/// </summary> 
/// <typeparam name="TEvent">The type of the domain event to be handled.</typeparam>
public interface IDomainEventHandler<in TEvent> : INotificationHandler<TEvent>
    where TEvent : IDomainEvent
{
}