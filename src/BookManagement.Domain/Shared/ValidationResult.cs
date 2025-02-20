namespace BookManagement.Domain.Shared;

/// <summary> 
/// Represents a validation result containing errors. 
/// </summary>
public sealed class ValidationResult : Result, IValidationResult
{
    #region Constructors
    
    // Constructor to initialize ValidationResult with errors
    private ValidationResult(Error[] errors)
        : base(false, IValidationResult.ValidationError)
        => Errors = errors;
    
    #endregion

    #region Properties
    
    // Array of errors that occurred during validation
    public Error[] Errors { get; }
    
    #endregion

    #region Methods
    
    // Factory method to create a ValidationResult with errors
    public static ValidationResult WithErrors(Error[] errors) => new(errors);
    
    #endregion
}