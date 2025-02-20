using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetUserWithRolesById;

public sealed record GetUserWithRolesByIdQuery(Guid UserId) : IQuery<UserWithRolesResponse>;