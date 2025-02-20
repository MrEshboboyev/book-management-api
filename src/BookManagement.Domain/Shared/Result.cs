namespace BookManagement.Domain.Shared;

public class Result
{
    #region Constructors
    
    // Constructor to initialize the Result object
    // Ensures that either isSuccess is true with no error, or isSuccess is false with an error
    protected Result(
        bool isSuccess,
        Error error)
    {
        switch (isSuccess)
        {
            case true when error != Error.None:
            case false when error == Error.None:
                throw new InvalidOperationException();
            default:
                IsSuccess = isSuccess;
                Error = error;
                break;
        }
    }
    
    #endregion

    #region Public fields (IsSuccess and IsFailure)
    
    // Indicates if the result is a success
    public bool IsSuccess { get; }

    // // Indicates if the result is a failure
    public bool IsFailure => !IsSuccess;
    
    #endregion

    #region Associated properties
    
    // Represents the error associated with the result
    public Error Error { get; }
    
    #endregion

    #region Static factory methods
    
    // Returns a successful Result
    public static Result Success() => new(true, Error.None);
    
    // Returns a successful Result with a value
    public static Result<TValue> Success<TValue>(TValue value)
        => new(value, true, Error.None);

    // Returns a failed Result with an error
    public static Result Failure(Error error) => new(false, error);
    
    // Returns a failed Result with a value and an error
    public static Result<TValue> Failure<TValue>(Error error)
        => new(default!, false, error);

    // Creates a Result based on the value;
    // if the value is not null, returns Success, otherwise Failure
    public static Result<TValue> Create<TValue>(TValue value)
        => value is not null ? Success(value) : Failure<TValue>(Error.NullValue);
    #endregion
}