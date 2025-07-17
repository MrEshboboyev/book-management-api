using Mapster;
using MapsterMapper;
using System.Reflection;

namespace BookManagement.App.Configurations;

public class MappingServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // 1. Create a new TypeAdapterConfig instance.
        // This will hold all your Mapster mapping configurations.
        var config = new TypeAdapterConfig();

        // 2. Scan assemblies for IRegister implementations.
        // This is where Mapster discovers your mapping classes (e.g., UserMappingRegister).
        // You should scan the assembly (or assemblies) where your IRegister classes reside.
        //
        // Common scenarios:
        // a) If mappings are in the same assembly as this installer or your main project:
        config.Scan(Assembly.GetExecutingAssembly());

        // b) If mappings are in a separate "Application" or "Domain" project/assembly:
        //    You would reference a type from that assembly to get its Assembly instance.
        //    Example: config.Scan(typeof(YourProjectNamespace.Application.AssemblyReference).Assembly);
        //    Or: config.Scan(Assembly.Load("YourProjectNamespace.Application"));
        //    Make sure to add appropriate 'using' directives if using specific types from other assemblies.

        // 3. Register the TypeAdapterConfig as a singleton.
        // It's a configuration object, so it's typically a singleton.
        services.AddSingleton(config);

        // 4. Register IMapper as a scoped service.
        // IMapper is the interface you'll inject into your handlers/services to perform mappings.
        // ServiceMapper is Mapster's default implementation.
        services.AddScoped<IMapper, ServiceMapper>();

        // Optional: If you want to use the static Adapt<TDestination>(this object source) extension
        // without injecting IMapper everywhere, you can configure global settings.
        // However, injecting IMapper is generally preferred for testability and DI consistency.
        // TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
    }
}
