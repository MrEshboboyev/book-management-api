using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetUserById;

public sealed record GetUserByIdQuery(Guid UserId) : IQuery<UserResponse>;