using BookManagement.Application.Abstractions.Messaging;

namespace BookManagement.Application.Users.Commands.CreateUser;

/// <summary>
/// Command to create a new user.
/// </summary>
/// <param name="Email">The email address of the new user.</param>
/// <param name="Password">The password for the new user.</param>
/// <param name="FirstName">The first name of the new user.</param>
/// <param name="LastName">The last name of the new user.</param>
public sealed record CreateUserCommand(
    string Email,
    string Password,
    string FirstName,
    string LastName) : ICommand<Guid>;