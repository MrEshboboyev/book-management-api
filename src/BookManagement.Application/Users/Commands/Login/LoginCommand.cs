using BookManagement.Application.Common.Messaging;

namespace BookManagement.Application.Users.Commands.Login;

/// <summary>
/// Command to log in a user.
/// </summary>
/// <param name="Email">The email address of the user attempting to log in.</param>
/// <param name="Password">The password of the user attempting to log in.</param>
public record LoginCommand(
    string Email,
    string Password) : ICommand<string>;