namespace BookManagement.Domain.Primitives;

/// <summary>
/// Represents a base class for value objects in DDD (Domain-Driven Design).
/// Value objects are immutable and represent a conceptual whole by defining their equality based on their properties.
/// </summary>
public abstract class ValueObject : IEquatable<ValueObject>
{
    #region Protected methods
    
    /// <summary>
    /// Gets the atomic values that define the equality of the value object.
    /// </summary>
    protected abstract IEnumerable<object> GetAtomicValues();
    
    #endregion
    
    #region Methods

    /// <summary>
    /// Determines whether the specified value object is equal to the current value object.
    /// </summary>
    public bool Equals(ValueObject other)
    {
        return other is not null && ValuesAreEqual(other);
    }
    
    #endregion
    
    #region Overrides

    /// <summary>
    /// Determines whether the specified object is equal to the current value object.
    /// </summary>
    public override bool Equals(object obj)
    {
        return obj is ValueObject other && ValuesAreEqual(other);
    }

    /// <summary>
    /// Returns a hash code for the value object.
    /// </summary>
    public override int GetHashCode()
    {
        return GetAtomicValues().Aggregate(default(int), HashCode.Combine);
    }
    
    #endregion
    
    #region Private methods

    /// <summary>
    /// Checks if the atomic values of the current value object are equal to the specified value object's atomic values.
    /// </summary>
    private bool ValuesAreEqual(ValueObject other)
    {
        return GetAtomicValues().SequenceEqual(other.GetAtomicValues());
    }
    
    #endregion
}