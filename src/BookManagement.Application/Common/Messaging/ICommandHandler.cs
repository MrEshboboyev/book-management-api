using BookManagement.Domain.Shared;
using MediatR;

namespace BookManagement.Application.Common.Messaging;

/// <summary> 
/// Handles a command without a specific response. 
/// Command handlers contain the business logic to process commands. 
/// </summary> 
/// <typeparam name="TCommand">The type of the command to be handled.</typeparam>
public interface ICommandHandler<in TCommand> : IRequestHandler<TCommand, Result>
    where TCommand : ICommand
{
}

/// <summary> 
/// Handles a command and returns a specific response. 
/// Command handlers contain the business logic to process commands and return results. 
/// </summary> 
/// <typeparam name="TCommand">The type of the command to be handled.</typeparam> 
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface ICommandHandler<in TCommand, TResponse> : IRequestHandler<TCommand, Result<TResponse>>
    where TCommand : ICommand<TResponse>
{
}