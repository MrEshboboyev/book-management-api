using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Queries.SearchUsers;

internal sealed class SearchUsersQueryHandler(IUserRepository userRepository)
    : IQueryHandler<SearchUsersQuery, UserListResponse>
{
    public async Task<Result<UserListResponse>> Handle(
        SearchUsersQuery request,
        CancellationToken cancellationToken)
    {
        var (email, name, roleId) = request;
        
        var users = await userRepository.SearchAsync(
            email,
            name,
            roleId,
            cancellationToken);
        
        var response = new UserListResponse(
            users
                .Select(UserResponseFactory.Create)
                .ToList());
        
        return Result.Success(response);
    }
}
