using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;

namespace BookManagement.App.Middlewares;

/// <summary> 
/// Middleware for globally handling exceptions during HTTP request processing. 
/// </summary>
public class GlobalExceptionHandlingMiddleware : IMiddleware
{
    private readonly ILogger<GlobalExceptionHandlingMiddleware> _logger;

    /// <summary> 
    /// Initializes a new instance of the <see cref="GlobalExceptionHandlingMiddleware"/> class. 
    /// </summary> 
    /// <param name="logger">The logger to log exceptions.</param>
    public GlobalExceptionHandlingMiddleware(ILogger<GlobalExceptionHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    /// <summary> 
    /// Processes an incoming HTTP request and handles any exceptions that occur. 
    /// </summary> 
    /// <param name="context">The HTTP context.</param> 
    /// <param name="next">The delegate to invoke the next middleware in the pipeline.</param>
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            // Invoke the next middleware in the pipeline.
            await next(context);
        }
        catch (Exception ex)
        {
            // Log the exception.
            _logger.LogError(ex, message: ex.Message);

            // Set the response status code to 500 Internal Server Error.
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            // Create a ProblemDetails object to represent the error.
            ProblemDetails problem = new()
            {
                Status = (int)HttpStatusCode.InternalServerError,
                Type = "Server error",
                Title = "Server error",
                Detail = "An internal server error has occured"
            };

            // Serialize the ProblemDetails object to JSON.
            var json = JsonSerializer.Serialize(problem);

            // Set the response content type to application/json.
            context.Response.ContentType = "application/json";

            // Write the JSON error response to the HTTP response.
            await context.Response.WriteAsync(json);
        }
    }
}