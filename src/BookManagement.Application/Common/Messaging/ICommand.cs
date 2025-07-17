using BookManagement.Domain.Shared;
using MediatR;

namespace BookManagement.Application.Common.Messaging;

/// <summary>
/// Represents a command in the application.
/// Commands are used to perform an action or change in the state of the application.
/// </summary>
public interface ICommand : IRequest<Result>
{
}

/// <summary>
/// Represents a command in the application that returns a specific response.
/// Commands are used to perform an action or change in the state of the application and return a result.
/// </summary>
/// <typeparam name="TResponse">Defining Command Return Type</typeparam>
public interface ICommand<TResponse> : IRequest<Result<TResponse>>
{
}