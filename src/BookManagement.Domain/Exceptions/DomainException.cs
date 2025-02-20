namespace BookManagement.Domain.Exceptions;

/// <summary> 
/// Represents a base class for all domain-specific exceptions in the BookManagement domain. 
/// Domain exceptions are used to handle errors and exceptional conditions that occur within the domain logic. 
/// </summary>
public abstract class DomainException : Exception
{
    /// <summary> 
    /// Initializes a new instance of the <see cref="DomainException"/> class with a specified error message. 
    /// </summary> 
    /// <param name="message">The message that describes the error.</param>
    protected DomainException(string message) : base(message)
    {

    }
}