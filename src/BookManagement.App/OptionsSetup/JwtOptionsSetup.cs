using BookManagement.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace BookManagement.App.OptionsSetup;

/// <summary>
/// Configures JWT options from the application configuration.
/// </summary>
/// <param name="configuration">The application configuration.</param>
public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";

    /// <summary>
    /// Configures the JWT options by binding them to the corresponding section in the configuration.
    /// </summary>
    /// <param name="options">The JWT options to configure.</param>
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}