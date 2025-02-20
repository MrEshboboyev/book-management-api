using BookManagement.Domain.Shared;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BookManagement.Application.Behaviors;

/// <summary> 
/// Pipeline behavior that logs the beginning, completion, and any failures of a request. 
/// </summary> 
/// <typeparam name="TRequest">The type of the request.</typeparam> 
/// <typeparam name="TResponse">The type of the response.</typeparam>
public class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest<TResponse>
    where TResponse : Result
{
    #region Private properties
    
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;
    
    #endregion

    #region Constructors
    
    /// <summary> 
    /// Initializes a new instance of the <see cref="LoggingPipelineBehavior{TRequest, TResponse}"/> class. 
    /// </summary> 
    /// <param name="logger">The logger to log information and errors.</param>
    public LoggingPipelineBehavior(
        ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger)
    {
        _logger = logger;
    }
    
    #endregion

    #region Handlers
    
    /// <summary> 
    /// Handles the logging of the request, its completion, and any failures. 
    /// </summary> 
    /// <param name="request">The request to handle.</param> 
    /// <param name="next">The next handler or behavior in the pipeline.</param> 
    /// <param name="cancellationToken">Cancellation token.</param> 
    /// <returns>The response from the next handler or behavior.</returns>
    public async Task<TResponse> Handle(
        TRequest request,
        RequestHandlerDelegate<TResponse> next,
        CancellationToken cancellationToken)
    {
        // Log the start of the request
        _logger.LogInformation(
            "Starting request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        // Handle the request and get the response
        var result = await next();

        // Log any failure that occurred during the request handling
        if (result.IsFailure)
        {
            _logger.LogError(
                "Request failure {@RequestName}, {@Error}, {@DateTimeUtc}",
                typeof(TRequest).Name,
                result.Error,
                DateTime.UtcNow);
        }

        // Log the completion of the request
        _logger.LogInformation(
            "Completed request {@RequestName}, {@DateTimeUtc}",
            typeof(TRequest).Name,
            DateTime.UtcNow);

        return result;
    }
    
    #endregion
}
