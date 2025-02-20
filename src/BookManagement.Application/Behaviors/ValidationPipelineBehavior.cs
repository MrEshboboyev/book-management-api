using FluentValidation;
using BookManagement.Domain.Shared;
using MediatR;

namespace BookManagement.Application.Behaviors;

/// <summary> 
/// Implements a validation behavior in the MediatR pipeline. 
/// </summary> 
/// <typeparam name="TRequest">The type of the request.</typeparam> 
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class ValidationPipelineBehavior<TRequest, TResponse>
    (IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    #region Handlers
    
    // Handles the validation of the request and returns validation errors if any
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // validators are not exist, await next()
        if (!validators.Any())
        {
            return await next();
        }

        // Validate the request and collect errors
        var errors = validators
            .Select(validator => validator.Validate(request))
            .SelectMany(validationResult => validationResult.Errors)
            .Where(validationFailure => validationFailure is not null)
            .Select(failure => new Error(
                failure.PropertyName,
                failure.ErrorMessage))
            .Distinct()
            .ToArray();

        // errors are found, return result with errors
        if (errors.Length != 0)
        {
            return CreateValidationResult<TResponse>(errors);
        }

        // await next()
        return await next();
    }
    
    #endregion
    
    #region Helpers

    // Creates a ValidationResult or ValidationResult<T> based on errors
    private static TResult CreateValidationResult<TResult>(Error[] errors)
        where TResult : Result
    {
        // Check if TResult is the non-generic Result type
        if (typeof(TResult) == typeof(Result))
        {
            return (ValidationResult.WithErrors(errors) as TResult)!;
        }

        // Check if TResult is a generic type and process accordingly
        if (!typeof(TResult).IsGenericType) 
            throw new InvalidOperationException("Unsupported result type.");
        
        var genericType = typeof(TResult).GetGenericArguments()[0];

        var validationResult = typeof(ValidationResult<>)
            .MakeGenericType(genericType) // Use the actual generic argument of TResult
            .GetMethod(nameof(ValidationResult.WithErrors))!
            .Invoke(null, [errors])!;

        return (TResult)validationResult;
    }
    
    #endregion
}