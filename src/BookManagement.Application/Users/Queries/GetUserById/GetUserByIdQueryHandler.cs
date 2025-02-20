using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.Shared;

namespace BookManagement.Application.Users.Queries.GetUserById;

/// <summary>
/// Handles the query to get a user by their unique identifier.
/// </summary>
internal sealed class GetUserByIdQueryHandler(IUserRepository userRepository)
    : IQueryHandler<GetUserByIdQuery, UserResponse>
{
    /// <summary>
    /// Processes the GetUserByIdQuery and retrieves the user details.
    /// </summary>
    /// <param name="request">The query request containing the user's unique identifier.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the user details if found, or an error.</returns>
    public async Task<Result<UserResponse>> Handle(GetUserByIdQuery request,
        CancellationToken cancellationToken)
    {
        #region Get User By Id

        // Retrieve the user by their unique identifier
        var user = await userRepository.GetByIdAsync(
            request.UserId,
            cancellationToken);

        // Check if the user is found
        if (user is null)
        {
            return Result.Failure<UserResponse>(
                DomainErrors.User.NotFound(request.UserId));
        }

        #endregion

        #region Prepare Response

        // Prepare the response with the user's details
        var response = new UserResponse(
            user.Id,
            user.Email.Value,
            user.FirstName.Value,
            user.LastName.Value);

        #endregion

        // Return the user details as a successful result
        return Result.Success(response);
    }
}