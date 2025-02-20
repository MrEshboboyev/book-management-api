using BookManagement.Domain.Entities.Users;

namespace BookManagement.Application.Abstractions.Security;

/// <summary>
/// Provides functionality for generating JSON Web Tokens (JWT) for authenticated users.
/// </summary>
public interface IJwtProvider
{
    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    Task<string> GenerateAsync(User user);
}