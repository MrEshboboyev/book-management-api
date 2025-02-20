namespace BookManagement.Presentation.Contracts.Users;

/// <summary>
/// Represents a request for user login.
/// </summary>
/// <param name="Email">The email address of the user attempting to log in.</param>
/// <param name="Password">The password of the user attempting to log in.</param>
public record LoginRequest(
    string Email,
    string Password);