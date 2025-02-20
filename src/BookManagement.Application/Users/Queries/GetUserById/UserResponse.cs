namespace BookManagement.Application.Users.Queries.GetUserById;

/// <summary>
/// Represents a response containing user details.
/// </summary>
/// <param name="Id">The unique identifier of the user.</param>
/// <param name="Email">The email address of the user.</param>
/// <param name="FirstName">The first name of the user.</param>
/// <param name="LastName">The last name of the user.</param>
public sealed record UserResponse(
    Guid Id,
    string Email,
    string FirstName,
    string LastName);