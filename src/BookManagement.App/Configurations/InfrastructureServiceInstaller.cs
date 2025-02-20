using BookManagement.Infrastructure;
using Scrutor;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs infrastructure services and configuration.
/// </summary>
public class InfrastructureServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the infrastructure services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        services
            .Scan(
                selector => selector
                    .FromAssemblies(
                        AssemblyReference.Assembly,
                        Persistence.AssemblyReference.Assembly)
                    .AddClasses(false)
                    .UsingRegistrationStrategy(RegistrationStrategy.Skip)
                    .AsMatchingInterface()
                    .WithScopedLifetime());
    }
}