namespace BookManagement.Domain.Shared;

public sealed class Error(string code, string message) : IEquatable<Error>
{
    #region Readonly members (None, NullValue)
    
    // None : empty code and empty message
    public static readonly Error None = new(string.Empty, string.Empty);

    // NullValue : specified result is nullable
    public static readonly Error NullValue = new(
        "Error.NullValue", 
        "The specified result is null");
    
    #endregion

    #region Public Fields
    
    // Code : defining an error code (Example : Member.NotFound)
    public string Code { get; } = code;

    // Message : defining an error message (Example : $"Member not found with Id {memberId}")
    public string Message { get; } = message;

    #endregion

    #region Implicit operators
    
    // allowing for seamless conversions between "Error" types and strings
    /* Using Example
        Error error = new Error { Code = "404 Not Found" }; 
        string errorMessage = error; // Implicitly converts to "404 Not Found"
    */
    public static implicit operator string(Error error)
        => error.Code;
    
    #endregion

    #region Equality operators
    
    // Equality Operator (==)
    // This method defines the equality operator (==) for the Error class,
    // allowing you to compare two Error objects to see if they are equal.
    public static bool operator ==(Error a, Error b)
    {
        if (a is null && b is null)
        {
            return true;
        }
        if (a is null || b is null)
        {
            return false;
        }
        return a.Equals(b);
    }

    // Inequality Operator (!=)
    // This method defines the inequality operator (!=) for the Error class,
    // allowing you to compare two Error objects to see if they are not equal.
    public static bool operator !=(Error a, Error b) => !(a == b);
    
    #endregion

    #region Equals() methods (virtual, override)
    
    /// <summary>
    /// This method checks if the other Error object is null. 
    /// If it is, it returns false because you cannot compare to null. 
    /// If other is not null, it then checks if both the Code and Message properties of the current object 
    /// and the other object are equal.
    /// </summary>
    public bool Equals(Error other)
    {
        if (other is null)
        {
            return false;
        }
        
        return Code == other.Code && Message == other.Message;
    }

    /// <summary>
    /// This method first checks if the obj parameter is of type Error. 
    /// If it is, it calls the Equals(Error other) method to perform the comparison. 
    /// If obj is not an Error, the method returns false.
    /// </summary>
    public override bool Equals(object obj) => obj is Error error && Equals(error);
    
    #endregion

    #region Overrides
    
    /// <summary>
    /// This method combines the hash codes of the Code and Message properties into a single hash code using 
    /// the HashCode.Combine method. This is useful for using Error objects in hash-based collections 
    /// like dictionaries or hash sets.
    /// </summary>
    public override int GetHashCode() => HashCode.Combine(Code, Message);

    /// <summary>
    /// This method returns the Code property as the string representation of the Error object. 
    /// This can be useful for logging or displaying the error code in user interfaces.
    /// </summary>
    public override string ToString() => Code;
    
    #endregion
}