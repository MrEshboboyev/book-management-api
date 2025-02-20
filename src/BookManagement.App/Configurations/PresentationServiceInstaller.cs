using BookManagement.Presentation;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs presentation services and configuration.
/// </summary>
public class PresentationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the presentation services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add controllers and application part
        services
            .AddControllers()
            .AddApplicationPart(AssemblyReference.Assembly);

        // Add OpenAPI (Swagger) support
        services.AddOpenApi();
    }
}