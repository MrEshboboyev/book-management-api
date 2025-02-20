using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Queries.GetUserById;

internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;
        
        // Fetch the user from the repository
        var user = await userRepository.GetByIdAsync(userId, cancellationToken);   

        // If user is not found, return a failure result
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFound(userId));
        }
        
        #region Prepare response
        
        var response = UserResponseFactory.Create(user);
        
        #endregion

        return Result.Success(response);
    }
}
