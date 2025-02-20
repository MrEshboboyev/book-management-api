using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetUserRoles;

public sealed record GetUserRolesQuery(Guid UserId) : IQuery<RoleListResponse>;
