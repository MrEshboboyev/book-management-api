using BookManagement.Application.Common.Data;
using BookManagement.Application.Common.Mappings;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Common.Pagination;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetAllUsers;
using BookManagement.Domain.Shared;
using Mapster;
using MapsterMapper;
using Microsoft.EntityFrameworkCore;

internal sealed class GetAllUsersQueryHandler(
    IApplicationDbContext context,
    IMapper mapper
) : IQueryHandler<GetAllUsersQuery, PaginatedList<UserResponse>>
{
    public async Task<Result<PaginatedList<UserResponse>>> Handle(
        GetAllUsersQuery request,
        CancellationToken cancellationToken)
    {
        var paginatedUsers = await context.Users
            .AsNoTracking()
            .OrderBy(u => u.CreatedOnUtc)
            .ProjectToType<UserResponse>(mapper.Config)
            .PaginatedListAsync(request.PageNumber, request.PageSize, cancellationToken);

        return Result.Success(paginatedUsers);
    }
}
