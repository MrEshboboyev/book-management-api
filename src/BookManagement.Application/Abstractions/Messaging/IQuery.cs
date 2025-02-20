using BookManagement.Domain.Shared;
using MediatR;

namespace BookManagement.Application.Abstractions.Messaging;

/// <summary> 
/// Represents a query in the application. 
/// Queries are used to retrieve data without changing the state of the application. 
/// </summary> 
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQuery<TResponse> : IRequest<Result<TResponse>>
{
}