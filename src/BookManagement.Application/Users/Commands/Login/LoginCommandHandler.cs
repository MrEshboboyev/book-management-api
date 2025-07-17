using BookManagement.Domain.Errors;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.ValueObjects.Users;
using BookManagement.Application.Common.Messaging;
using BookManagement.Application.Common.Security;

namespace BookManagement.Application.Users.Commands.Login;

/// <summary>
/// Handles the command to log in a user.
/// </summary>
internal sealed class LoginCommandHandler(
    IUserRepository userRepository,
    IJwtProvider jwtProvider,
    IPasswordHasher passwordHasher) : ICommandHandler<LoginCommand, string>
{
    /// <summary>
    /// Processes the LoginCommand and logs in the user by generating a JWT.
    /// </summary>
    /// <param name="request">The command request containing the user's email and password.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the JWT token if successful or an error.</returns>
    public async Task<Result<string>> Handle(LoginCommand request,
        CancellationToken cancellationToken)
    {
        #region Checking user exists by this email

        // Validate and create the Email value object
        Result<Email> createEmailResult = Email.Create(request.Email);
        if (createEmailResult.IsFailure)
        {
            return Result.Failure<string>(
                createEmailResult.Error);
        }

        // Retrieve the user by email
        var user = await userRepository.GetByEmailAsync(
            createEmailResult.Value,
            cancellationToken);

        // Verify if user exists and the password matches
        if (user is null || !passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return Result.Failure<string>(
                DomainErrors.User.InvalidCredentials);
        }

        #endregion

        #region Generate token

        // Generate a JWT token for the authenticated user
        var token = await jwtProvider.GenerateAsync(user);

        #endregion

        // Return the generated token
        return Result.Success(token);
    }
}