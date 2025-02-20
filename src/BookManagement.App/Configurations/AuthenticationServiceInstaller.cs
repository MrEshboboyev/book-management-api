using BookManagement.App.OptionsSetup;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs authentication services and configuration.
/// </summary>
public class AuthenticationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the authentication services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Configure JWT options setup
        services.ConfigureOptions<JwtOptionsSetup>();
        services.ConfigureOptions<JwtBearerOptionsSetup>();

        // Add authentication services with JWT Bearer authentication scheme
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer();
    }
}