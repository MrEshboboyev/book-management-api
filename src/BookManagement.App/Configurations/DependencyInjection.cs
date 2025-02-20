using System.Reflection;

namespace BookManagement.App.Configurations;

/// <summary> 
/// Provides extension methods for service registration. 
/// </summary>
public static class DependencyInjection
{
    /// <summary> 
    /// Scans assemblies for types implementing IServiceInstaller and registers their services. 
    /// </summary> 
    /// <param name="services">The IServiceCollection to add services to.</param> 
    /// <param name="configuration">The IConfiguration for configuration settings.</param> 
    /// <param name="assemblies">Assemblies to scan for IServiceInstaller implementations.</param> 
    /// <returns>The IServiceCollection with registered services.</returns>
    public static IServiceCollection InstallServices(
        this IServiceCollection services,
        IConfiguration configuration,
        params Assembly[] assemblies)
    {
        // Scan provided assemblies for types implementing IServiceInstaller
        var serviceInstallers = assemblies
             .SelectMany(a => a.DefinedTypes)
             .Where(IsAssignableToType<IServiceInstaller>)
             .Select(Activator.CreateInstance)
             .Cast<IServiceInstaller>();

        // Register services using each IServiceInstaller implementation
        foreach (var serviceInstaller in serviceInstallers)
        {
            serviceInstaller.Install(services, configuration);
        }

        return services;

        // Helper method to check if a type implements a specific interface
        static bool IsAssignableToType<T>(TypeInfo typeInfo) =>
          typeof(T).IsAssignableFrom(typeInfo) &&
          typeInfo is { IsInterface: false, IsAbstract: false };
    }
}