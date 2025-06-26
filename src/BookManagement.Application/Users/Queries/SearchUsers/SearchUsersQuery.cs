using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.SearchUsers;

public sealed record SearchUsersQuery(
    string Email = null,
    string Name = null,
    int? RoleId = null) : IQuery<UserListResponse>;