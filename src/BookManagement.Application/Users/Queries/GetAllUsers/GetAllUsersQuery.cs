using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery() : IQuery<UserListResponse>;