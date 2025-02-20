using BookManagement.Application.Abstractions.Security;
using BookManagement.Application.Abstractions.Messaging;
using BookManagement.Domain.Errors;
using BookManagement.Domain.Repositories;
using BookManagement.Domain.Shared;
using BookManagement.Domain.Entities.Users;
using BookManagement.Domain.Repositories.Users;
using BookManagement.Domain.ValueObjects.Users;

namespace BookManagement.Application.Users.Commands.RegisterUser;

/// <summary>
/// Handles the command to Register a new user.
/// </summary>
internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository,
    IUnitOfWork unitOfWork,
    IPasswordHasher passwordHasher)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    /// <summary>
    /// Processes the RegisterUserCommand and Registers a new user.
    /// </summary>
    /// <param name="request">The command request containing user details.</param>
    /// <param name="cancellationToken">Optional cancellation token.</param>
    /// <returns>A Result containing the unique identifier of the Created user or an error.</returns>
    public async Task<Result<Guid>> Handle(RegisterUserCommand request,
        CancellationToken cancellationToken)
    {
        #region Checking Email is Unique

        // Validate and Register the Email value object
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

        #region Get Role

        var role = await roleRepository.GetByIdAsync(Role.Registered.Id, cancellationToken);
        if (role is null)
        {
            return Result.Failure<Guid>(
                DomainErrors.Role.NotFound(Role.Registered.Id));
        }

        #endregion

        #region Password hashing

        // Hash the user's password
        var passwordHash = passwordHasher.Hash(request.Password);

        #endregion

        #region Register new user

        // Register a new User entity with the provided details
        var user = User.Create(
            Guid.NewGuid(),
            emailResult.Value,
            passwordHash,
            createFirstNameResult.Value,
            createLastNameResult.Value,
            role);

        #endregion

        #region Add and Update database

        // Add the new user to the repository and save changes
        await userRepository.AddAsync(user, cancellationToken);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        #endregion

        // Return the unique identifier of the newly Registerd user
        return Result.Success(user.Id);
    }
}