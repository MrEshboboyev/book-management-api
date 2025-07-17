using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Users;

/// <summary> 
/// Represents an email value object. 
/// </summary>
public sealed class LastName : ValueObject
{
    #region Constants
    
    public const int MaxLength = 50; // Maximum length for an LastName
    
    #endregion

    #region Constructors
    
    private LastName(string value)
    {
        Value = value;
    }
    
    private LastName()
    {
    }
    
    #endregion
    
    #region Properties
    public string Value { get; }
    
    #endregion
    
    #region Factory Methods
    
    /// <summary> 
    /// Creates a LastName instance after validating the input. 
    /// </summary> 
    /// <param name="lastName">The last name string to create the LastName value object from.</param> 
    /// <returns>A Result object containing the LastName value object or an error.</returns>
    public static Result<LastName> Create(string lastName)
    {
        if (string.IsNullOrWhiteSpace(lastName))
        {
            return Result.Failure<LastName>(DomainErrors.LastName.Empty);
        }
        
        if (lastName.Length > MaxLength)
        {
            return Result.Failure<LastName>(DomainErrors.LastName.TooLong);
        }
        
        return Result.Success(new LastName(lastName));
    }

    #endregion

    #region Operators

    public static implicit operator string(LastName lastName) => lastName?.Value;

    public override string ToString() => Value;

    #endregion

    #region Overrides

    /// <summary> 
    /// Returns the atomic values of the LastName object for equality checks. 
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}