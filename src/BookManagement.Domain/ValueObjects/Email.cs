using BookManagement.Domain.Errors;
using BookManagement.Domain.Primitives;
using BookManagement.Domain.Shared;

namespace BookManagement.Domain.ValueObjects;

public sealed class Email : ValueObject
{
    #region Constants
    
    public const int MaxLength = 50; // Maximum length for an email
    
    #endregion
    
    #region Constructors
    
    private Email(string value)
    {
        Value = value;
    }
    
    #endregion
    
    #region Properties
    
    public string Value { get; }
    
    #endregion

    #region Factory Methods
    
    /// <summary> 
    /// Creates an Email instance after validating the input. 
    /// </summary> 
    /// <param name="email">The email string to create the Email value object from.</param> 
    /// <returns>A Result object containing the Email value object or an error.</returns>
    public static Result<Email> Create(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
        {
            return Result.Failure<Email>(DomainErrors.Email.Empty);
        }
        
        if (email.Split('@').Length != 2)
        {
            return Result.Failure<Email>(DomainErrors.Email.InvalidFormat);
        }
        
        return Result.Success(new Email(email));
    }
    
    #endregion
    
    #region Overrides
    
    /// <summary> 
    /// Returns the atomic values of the Email object for equality checks. 
    /// </summary>
    protected override IEnumerable<object> GetAtomicValues()
    {
        yield return Value;
    }
    
    #endregion
}