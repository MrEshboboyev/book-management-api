using BookManagement.Application.Abstractions.Security;
using BookManagement.Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Provides functionality for generating JSON Web Tokens (JWT) for authenticated users.
/// </summary>
internal sealed class JwtProvider(IOptions<JwtOptions> options) : IJwtProvider
{
    #region Private fields

    // Holds the JWT options configuration
    private readonly JwtOptions _options = options.Value;

    #endregion

    /// <summary>
    /// Generates a JWT for the specified user.
    /// </summary>
    /// <param name="user">The user for whom the token is being generated.</param>
    /// <returns>A JWT as a string.</returns>
    public string Generate(User user)
    {
        #region Create Claims List

        // Create a list of claims for the JWT
        var claims = new Claim[]
        {
            new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        #endregion

        #region Create signing credentials

        // Create signing credentials using the secret key from options
        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey)),
            SecurityAlgorithms.HmacSha256);

        #endregion

        #region New Jwt Security token

        // Create the JWT security token
        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        #endregion

        #region Create Jwt Token

        // Write the token to a string
        var tokenValue = new JwtSecurityTokenHandler().WriteToken(token);

        #endregion

        return tokenValue;
    }
}