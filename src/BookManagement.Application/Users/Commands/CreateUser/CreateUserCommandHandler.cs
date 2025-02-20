using BookManagement.Application.Abstractions.Security;
using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Entities;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Shared;
using BookManagement.Domain.ValueObjects;

namespace BookManagement.Application.Users.Commands.CreateUser;

/// <summary>
/// Handles the command to create a new user.
/// </summary>
internal sealed class CreateUserCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : ICommandHandler<CreateUserCommand, Guid>
{
    /// <summary>
    /// Processes the CreateUserCommand and creates a new user.
    /// </summary>
    /// <param name="request">The command request containing user details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the unique identifier of the created user or an error.</returns>
    public async Task<Result<Guid>> Handle(CreateUserCommand request,
        CancellationToken cancellationToken)
    {
        #region Checking Email is Unique

        // Validate and create the Email value object
        Result<Email> emailResult = Email.Create(request.Email);

        // Check if the email is already in use
        if (!await userRepository.IsEmailUniqueAsync(emailResult.Value, cancellationToken))
        {
            return Result.Failure<Guid>(DomainErrors.User.EmailAlreadyInUse);
        }

        #endregion

        #region Prepare value objects

        // Validate and create the FirstName value object
        Result<FirstName> createFirstNameResult = FirstName.Create(request.FirstName);
        if (createFirstNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        // Validate and create the LastName value object
        Result<LastName> createLastNameResult = LastName.Create(request.LastName);
        if (createLastNameResult.IsFailure)
        {
            return Result.Failure<Guid>(
                createFirstNameResult.Error);
        }

        #endregion

        #region Password hashing

        // Hash the user's password
        var passwordHash = passwordHasher.Hash(request.Password);

        #endregion

        #region Create new user

        // Create a new User entity with the provided details
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value);

        #endregion

        #region Add and Update database

        // Add the new user to the repository and save changes
        userRepository.Add(user);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        // Return the unique identifier of the newly created user
        return Result.Success(user.Id);
    }
}