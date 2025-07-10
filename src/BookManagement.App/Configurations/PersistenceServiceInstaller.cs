using BookManagement.Persistence;
using BookManagement.Persistence.Interceptors;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.App.Configurations;

public class PersistenceServiceInstaller : IServiceInstaller
{
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add the domain events to outbox messages interceptor as a singleton
        services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

        services.AddSingleton<UpdateAuditableEntitiesInterceptor>();
        services.AddSingleton<GenerateIdInterceptor>();

        services.AddDbContext<ApplicationDbContext>((sp, optionsBuilder) =>
        {
            optionsBuilder
                .UseNpgsql(configuration.GetConnectionString("PostgresDatabase"))
                .AddInterceptors(
                    sp.GetRequiredService<ConvertDomainEventsToOutboxMessagesInterceptor>(),
                    sp.GetRequiredService<UpdateAuditableEntitiesInterceptor>(),
                    sp.GetRequiredService<GenerateIdInterceptor>());
        });
    }
}
