using MediatR;

namespace BookManagement.Domain.Primitives;

/// <summary> 
/// Represents a domain event in the context of DDD (Domain-Driven Design). 
/// Domain events are used to communicate significant changes in the state of the domain. 
/// </summary>
public interface IDomainEvent : INotification
{
    /// <summary> 
    /// Gets the unique identifier of the domain event. 
    /// </summary>
    public Guid Id { get; init; }
}