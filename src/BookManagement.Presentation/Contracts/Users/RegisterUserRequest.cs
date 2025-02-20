namespace BookManagement.Presentation.Contracts.Users;

/// <summary>
/// Represents a request to register a new user.
/// </summary>
/// <param name="Email">The email address of the new user.</param>
/// <param name="Password">The password for the new user.</param>
/// <param name="FirstName">The first name of the new user.</param>
/// <param name="LastName">The last name of the new user.</param>
public sealed record RegisterUserRequest(
    string Email,
    string Password,
    string FirstName,
    string LastName);