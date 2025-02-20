using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Queries.GetUserWithRolesById;

internal sealed class GetUserWithRolesByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserWithRolesByIdQuery, UserWithRolesResponse>
{
    public async Task<Result<UserWithRolesResponse>> Handle(
        GetUserWithRolesByIdQuery request,
        CancellationToken cancellationToken)
    {
        var userId = request.UserId;

        // Fetch the user from the repository
        var user = await userRepository.GetByIdWithRolesAsync(userId, cancellationToken);

        // If user is not found, return a failure result
        if (user is null)
        {
            return Result.Failure<UserWithRolesResponse>(
                DomainErrors.User.NotFound(userId));
        }

        #region Prepare response

        var response = new UserWithRolesResponse(
            UserResponseFactory.Create(user),
                user.Roles
                    .Select(RoleResponseFactory.Create)
                    .ToList()
                    .AsReadOnly()
        );

        #endregion

        return Result.Success(response);
    }
}