using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetAllRoles;

public sealed record GetAllRolesQuery : IQuery<RoleListResponse>;
