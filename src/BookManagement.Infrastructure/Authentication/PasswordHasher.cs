using BookManagement.Application.Abstractions.Security;
using System.Security.Cryptography;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Provides functionality for hashing and verifying passwords using PBKDF2 with SHA512.
/// </summary>
internal sealed class PasswordHasher : IPasswordHasher
{
    #region Private fields

    #region Constants

    /// <summary>
    /// Size of the salt in bytes.
    /// </summary>
    private const int SaltSize = 16;

    /// <summary>
    /// Size of the hash in bytes.
    /// </summary>
    private const int HashSize = 32;

    /// <summary>
    /// Number of iterations for the PBKDF2 algorithm.
    /// </summary>
    private const int Iterations = 500000;

    #endregion

    #region Fields

    /// <summary>
    /// Hashing algorithm used for password hashing.
    /// </summary>
    private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

    #endregion

    #endregion

    #region Implementations

    /// <summary>
    /// Hashes the specified plain-text password using PBKDF2 with a generated salt.
    /// </summary>
    /// <param name="password">The plain-text password to hash.</param>
    /// <returns>The hashed password as a string, including the salt.</returns>
    public string Hash(string password)
    {
        // Generate a new salt
        var salt = RandomNumberGenerator.GetBytes(SaltSize);

        // Hash the password with the salt
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations,
            Algorithm, HashSize);

        // Return the hash and salt as a single string
        return $"{Convert.ToHexString(hash)}-{Convert.ToHexString(salt)}";
    }

    /// <summary>
    /// Verifies whether the specified plain-text password matches the given hashed password.
    /// </summary>
    /// <param name="password">The plain-text password to verify.</param>
    /// <param name="passwordHash">The hashed password to compare against.</param>
    /// <returns>True if the password matches the hash; otherwise, false.</returns>
    public bool Verify(string password, string passwordHash)
    {
        // Split the hashed password into hash and salt parts
        var parts = passwordHash.Split('-');
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        // Hash the input password with the original salt
        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations,
            Algorithm, HashSize);

        // Compare the input hash with the original hash
        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
    }

    #endregion
}