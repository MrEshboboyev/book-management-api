namespace BookManagement.Application.Abstractions.Security;

/// <summary>
/// Provides functionality for hashing passwords and verifying password hashes.
/// </summary>
public interface IPasswordHasher
{
    /// <summary>
    /// Hashes the specified plain-text password.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The hashed version of the password.</returns>
    string Hash(string password);

    /// <summary>
    /// Verifies whether the specified plain-text password matches the given password hash.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="passwordHash">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    bool Verify(string password, string passwordHash);
}