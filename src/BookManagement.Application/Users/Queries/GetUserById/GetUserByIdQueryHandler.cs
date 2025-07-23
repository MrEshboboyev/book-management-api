using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Users.Queries.Common.Factories;
using BookManagement.Application.Users.Queries.Common.Responses;
using BookManagement.Application.Users.Queries.GetUserById;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

internal sealed class GetUserByIdQueryHandler(
    IUserRepository userRepository
) : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    public async Task<Result<UserResponse>> Handle(
        GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        var user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);
        if (user is null)
        {
            return Result.Failure<UserResponse>(DomainErrors.User.NotFound(request.UserId));
        }

        var response = UserResponseFactory.Create(user);

        return Result.Success(response);
    }
}
