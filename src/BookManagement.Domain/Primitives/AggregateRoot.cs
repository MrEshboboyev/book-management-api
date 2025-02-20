namespace BookManagement.Domain.Primitives;

/// <summary>
/// Represents an aggregate root in DDD (Domain-Driven Design).
/// Aggregate roots are the entry point to aggregates, and they encapsulate domain logic and rules.
/// </summary>
public abstract class AggregateRoot : Entity
{
    #region Private fields
    
    // List of domain events associated with the aggregate root
    private readonly List<IDomainEvent> _domainEvents = [];

    #endregion

    #region Constructors
    
    // Constructor to initialize the aggregate root with an ID
    protected AggregateRoot(Guid id) : base(id)
    {
        
    }
    
    // Parameterless constructor for aggregate root
    protected AggregateRoot()
    {

    }
    
    #endregion

    #region Domain Events Methods

    // Gets the domain events associated with the aggregate root
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents()
        => [.. _domainEvents];

    // Clears the domain events associated with the aggregate root
    public void ClearDomainEvents() => _domainEvents.Clear();

    // Raises a domain event and adds it to the list of domain events
    protected void RaiseDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }
    
    #endregion
}