namespace BookManagement.Domain.Primitives;

/// <summary>
/// Represents a base class for entities in DDD (Domain-Driven Design). 
/// Entities have a distinct identity and are distinguishable from other entities by their ID.
/// </summary>
public abstract class Entity : IEquatable<Entity>
{
    #region Constructors
    
    // Constructor to initialize the entity with an ID
    protected Entity(Guid id) => Id = id;

    // Parameterless constructor for entity
    protected Entity()
    {
    }
    
    #endregion

    #region Properties
    
    // Unique identifier for the entity
    public Guid Id { get; protected init; }
    
    #endregion

    #region Equality Operators
    
    // Checks if two entities are equal by comparing their IDs
    public static bool operator ==(Entity first, Entity second) =>
        first is not null && second is not null && first.Equals(second);
    
    // Checks if two entities are not equal by comparing their IDs
    public static bool operator !=(Entity first, Entity second) =>
        !(first == second);
    
    #endregion

    #region Equality Methods
    
    // Checks if the specified entity is equal to the current entity
    public bool Equals(Entity other)
    {
        if (other is null)
        {
            return false;
        }
        
        if (other.GetType() != GetType())
        {
            return false;
        }
        
        return other.Id == Id;
    }

    // Checks if the specified object is equal to the current entity
    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }
        
        if (obj.GetType() != GetType())
        {
            return false;
        }
        
        if (obj is not Entity entity)
        {
            return false;
        }
        
        return entity.Id == Id;
    }

    // Returns a hash code for the entity based on its ID
    public override int GetHashCode()
        => Id.GetHashCode() * 41;
    
    #endregion
}