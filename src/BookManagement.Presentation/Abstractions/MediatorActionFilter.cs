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
            var request = CreateRequest(endpoint.RequestType, context.ActionArguments);
            var result = await sender.Send(request, context.HttpContext.RequestAborted);

            // Get the actual Result type via dynamic dispatch
            context.Result = HandleResultDynamic(result);
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

            if (actionArguments.TryGetValue(param.Name, out var value))
            {
                args[i] = value;
                continue;
            }

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

            args[i] = param.HasDefaultValue ? param.DefaultValue : null;
        }

        return Activator.CreateInstance(requestType, args);
    }

    #region Handle Result

    private static IActionResult HandleResultDynamic(object result)
    {
        var resultType = result.GetType();

        // Check if it's a Result<T> or Result
        if (resultType.IsGenericType && resultType.GetGenericTypeDefinition() == typeof(Result<>))
        {
            dynamic dynamicResult = result;
            return HandleTypedResult(dynamicResult);
        }
        else if (result is Result nonGenericResult)
        {
            return HandleNonGenericResult(nonGenericResult);
        }

        throw new InvalidOperationException($"Unexpected result type: {resultType}");
    }

    private static IActionResult HandleTypedResult<T>(Result<T> result)
    {
        return result.IsSuccess
            ? new OkObjectResult(result.Value)
            : HandleFailure(result);
    }

    private static IActionResult HandleNonGenericResult(Result result)
    {
        return result.IsSuccess
            ? new OkResult()
            : HandleFailure(result);
    }

    #endregion

    #region Handle Failure

    private static IActionResult HandleFailure(Result result)
    {
        if (result.IsSuccess)
            throw new InvalidOperationException("Cannot handle failure for successful result");

        if (result is IValidationResult validationResult)
        {
            return new BadRequestObjectResult(
                CreateProblemDetails(
                    "Validation Error",
                    StatusCodes.Status400BadRequest,
                    result.Error,
                    [.. validationResult.Errors]));
        }

        return new BadRequestObjectResult(
            CreateProblemDetails(
                "Bad Request",
                StatusCodes.Status400BadRequest,
                result.Error));
    }

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

    #endregion
}
