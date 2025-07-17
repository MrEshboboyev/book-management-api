using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Common.Pagination;
using BookManagement.Application.Users.Queries.Common.Responses;

namespace BookManagement.Application.Users.Queries.GetAllUsers;

public sealed record GetAllUsersQuery(
    int PageNumber = 1, // Default page number
    int PageSize = 10   // Default page size
) : IQuery<PaginatedList<UserResponse>>;
