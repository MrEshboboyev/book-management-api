using BookManagement.Domain.Shared;
using MediatR;

namespace BookManagement.Application.Abstractions.Messaging;

/// <summary> 
/// Handles a query and returns a specific response. 
/// Query handlers contain the business logic to process queries and return results. 
/// </summary> 
/// <typeparam name="TQuery">The type of the query to be handled.</typeparam> 
/// <typeparam name="TResponse">The type of the response.</typeparam>
public interface IQueryHandler<in TQuery, TResponse> : IRequestHandler<TQuery, Result<TResponse>>
    where TQuery : IQuery<TResponse>
{
}