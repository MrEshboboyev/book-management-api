using System.Security.Claims;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using BookManagement.Domain.Shared;

namespace BookManagement.Presentation.Abstractions;

/// <summary> 
/// Serves as a base class for all API controllers, providing common functionality. 
/// </summary>
[ApiController]
public abstract class ApiController : ControllerBase
{
    /// <summary> 
    /// The sender used for sending commands and queries through MediatR. 
    /// </summary>
    protected readonly ISender Sender;

    /// <summary> 
    /// Initializes a new instance of the <see cref="ApiController"/> class. 
    /// </summary> 
    /// <param name="sender">The sender used for sending commands and queries.</param>
    protected ApiController(ISender sender)
    {
        Sender = sender;
    }

    protected Guid GetUserId() =>
        Guid.Parse(
            User.FindFirst(ClaimTypes.NameIdentifier)!
                .Value);

    /// <summary> 
    /// Handles failure scenarios and returns appropriate HTTP responses. 
    /// </summary> 
    /// <param name="result">The result to handle.</param> 
    /// <returns>An IActionResult representing the HTTP response.</returns>
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            IValidationResult validationResult =>
                BadRequest(
                    CreateProblemDetails(
                        "Validation Error",
                        StatusCodes.Status400BadRequest,
                        result.Error, validationResult.Errors)),
            _ => BadRequest(
                CreateProblemDetails(
                    "Bad Request",
                    StatusCodes.Status400BadRequest,
                    result.Error))
        };

    /// <summary> 
    /// Creates a ProblemDetails object for detailed error responses. 
    /// </summary> 
    /// <param name="title">The title of the problem.</param> 
    /// <param name="status">The HTTP status code.</param> 
    /// <param name="error">The main error.</param> 
    /// <param name="errors">Optional additional errors.</param> 
    /// <returns>A ProblemDetails object with the specified details.</returns>
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[] errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status,
            Extensions =
            {
                {
                    nameof(errors),
                    errors
                }
            }
        };
}