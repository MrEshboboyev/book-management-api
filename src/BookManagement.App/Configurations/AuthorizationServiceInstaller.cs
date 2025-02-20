using BookManagement.Infrastructure.Authentication;
using Microsoft.AspNetCore.Authorization;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs authorization services and configuration.
/// </summary>
public class AuthorizationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the authorization services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add authorization services
        services.AddAuthorization();

        // Register the permission authorization handler
        services.AddSingleton<IAuthorizationHandler, PermissionAuthorizationHandler>();

        // Register the permission authorization policy provider
        services.AddSingleton<IAuthorizationPolicyProvider, PermissionAuthorizationPolicyProvider>();
    }
}