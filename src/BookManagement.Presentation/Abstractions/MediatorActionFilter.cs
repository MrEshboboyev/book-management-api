using BookManagement.Domain.Shared;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BookManagement.Presentation.Abstractions;

// Action filter to handle the common pattern
public class MediatorActionFilter(ISender sender) : IAsyncActionFilter
{
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var endpoint = context.ActionDescriptor.EndpointMetadata
            .OfType<MediatorEndpointAttribute>()
            .FirstOrDefault();

        if (endpoint == null)
        {
            await next();
            return;
        }

        try
        {
            // build the MediatR request from action args
            var request = CreateRequest(endpoint.RequestType, context.ActionArguments);

            var result = await sender.Send(request, context.HttpContext.RequestAborted);

            // turn Result<T> or Result into IActionResult
            context.Result = HandleResult(result, endpoint.IsCommand);
        }
        catch (Exception ex)
        {
            context.Result = new ObjectResult(new ProblemDetails
            {
                Title = "Internal Server Error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = ex.Message
            })
            {
                StatusCode = StatusCodes.Status500InternalServerError
            };
        }
    }

    private static object CreateRequest(Type requestType, IDictionary<string, object> actionArguments)
    {
        var ctor = requestType.GetConstructors().First();
        var parameters = ctor.GetParameters();
        var args = new object[parameters.Length];

        for (int i = 0; i < parameters.Length; i++)
        {
            var param = parameters[i];

            // Try exact parameter name match
            if (actionArguments.TryGetValue(param.Name, out var value))
            {
                args[i] = value;
                continue;
            }

            // For complex objects, try to find matching properties
            var complexObject = actionArguments.Values
                .FirstOrDefault(x => x?.GetType().IsClass == true && !(x is string));

            if (complexObject != null)
            {
                var prop = complexObject.GetType().GetProperty(param.Name);
                if (prop != null)
                {
                    args[i] = prop.GetValue(complexObject);
                    continue;
                }
            }

            // Fallback to default value if not found
            args[i] = param.HasDefaultValue ? param.DefaultValue : null;
        }

        return Activator.CreateInstance(requestType, args);
    }
    private IActionResult HandleResult(object result, bool isCommand)
    {
        var resultType = result.GetType();
        var isSuccess = (bool)resultType.GetProperty("IsSuccess").GetValue(result);
        var isFailure = (bool)resultType.GetProperty("IsFailure").GetValue(result);
        var valueProp = resultType.GetProperty("Value");
        var value = valueProp?.GetValue(result);

        if (isCommand)
        {
            return isFailure
                ? HandleFailure(result)
                : new OkObjectResult(value);
        }

        return isSuccess
            ? new OkObjectResult(value)
            : HandleFailure(result);
    }

    private IActionResult HandleFailure(object result)
    {
        var t = result.GetType();
        var isSuccess = (bool)t.GetProperty("IsSuccess").GetValue(result);
        if (isSuccess)
            throw new InvalidOperationException();

        var error = (Error)t.GetProperty("Error").GetValue(result);

        if (result is IValidationResult validationResult)
        {
            return new BadRequestObjectResult(
                CreateProblemDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    error,
                    validationResult.Errors.ToArray()));
        }

        return new BadRequestObjectResult(
            CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                error));
    }

    /// <summary>
    /// Creates a ProblemDetails object for detailed error responses.
    /// </summary>
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[] errors = null)
    {
        var pd = new ProblemDetails
        {
            Title = title,
            Type = error.Code,
            Detail = error.Message,
            Status = status
        };

        if (errors != null)
            pd.Extensions[nameof(errors)] = errors;

        return pd;
    }
}
