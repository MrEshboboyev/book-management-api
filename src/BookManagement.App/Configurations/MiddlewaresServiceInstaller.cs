using BookManagement.App.Middlewares;

namespace BookManagement.App.Configurations;

/// <summary> 
/// Implements the IServiceInstaller interface to register middleware services. 
/// </summary>
public class MiddlewaresServiceInstaller : IServiceInstaller
{
    /// <summary> 
    /// Installs middleware services into the IServiceCollection. 
    /// </summary> 
    /// <param name="services">The IServiceCollection to add services to.</param> 
    /// <param name="configuration">The IConfiguration for configuration settings.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Register the GlobalExceptionHandlingMiddleware as a transient service
        services.AddTransient<GlobalExceptionHandlingMiddleware>();
    }
}