namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Options for configuring JSON Web Token (JWT) authentication.
/// </summary>
public class JwtOptions
{
    /// <summary>
    /// Gets or sets the issuer of the JWT.
    /// </summary>
    public string Issuer { get; set; }

    /// <summary>
    /// Gets or sets the audience for the JWT.
    /// </summary>
    public string Audience { get; set; }

    /// <summary>
    /// Gets or sets the secret key used for signing the JWT.
    /// </summary>
    public string SecretKey { get; set; }
}