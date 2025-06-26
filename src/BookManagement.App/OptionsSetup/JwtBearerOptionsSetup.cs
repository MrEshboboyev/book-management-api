using BookManagement.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace BookManagement.App.OptionsSetup;

/// <summary>
/// Configures JWT Bearer options for authentication.
/// </summary>
/// <param name="jwtOptions">The JWT options for configuration.</param>
public class JwtBearerOptionsSetup(IOptions<JwtOptions> jwtOptions)
    : IPostConfigureOptions<JwtBearerOptions>
{
    private readonly JwtOptions _jwtOptions = jwtOptions.Value;

    /// <summary>
    /// Configures the JWT Bearer options post configuration.
    /// </summary>
    /// <param name="name">The name of the options instance being configured.</param>
    /// <param name="options">The JWT Bearer options to configure.</param>
    public void PostConfigure(string name, JwtBearerOptions options)
    {
        options.TokenValidationParameters.ValidIssuer = _jwtOptions.Issuer;
        options.TokenValidationParameters.ValidAudience = _jwtOptions.Audience;
        options.TokenValidationParameters.IssuerSigningKey =
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOptions.SecretKey));
    }
}