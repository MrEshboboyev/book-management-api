using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects.Users;

public sealed class FirstName : ValueObject
{
    #region Constants
    
    public const int MaxLength = 50; // Maximum length for an FirstName
    
    #endregion
    
    #region Constructors
    
    private FirstName(string value)
    {
        Value = value;
    }
    
    #endregion
    
    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Methods

    /// <summary> 
    /// Creates a FirstName instance after validating the input. 
    /// </summary> 
    /// <param name="firstName">The first name string to create the FirstName value object from.</param> 
    /// <returns>A Result object containing the FirstName value object or an error.</returns>
    public static Result<FirstName> Create(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
        {
            return Result.Failure<FirstName>(DomainErrors.FirstName.Empty);
        }
        
        if (firstName.Length > MaxLength)
        {
            return Result.Failure<FirstName>(DomainErrors.FirstName.TooLong);
        }
        
        return Result.Success(new FirstName(firstName));
    }
    
    #endregion

    #region Overrides
    
    /// <summary> 
    /// Returns the atomic values of the FirstName object for equality checks. 
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}