using BookManagement.Persistence.Interceptors;
using BookManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.App.Configurations
{
    /// <summary>
    /// Installs persistence services and configuration.
    /// </summary>
    public class PersistenceServiceInstaller : IServiceInstaller
    {
        /// <summary>
        /// Configures the persistence services.
        /// </summary>
        /// <param name="services">The collection of services to configure.</param>
        /// <param name="configuration">The application configuration.</param>
        public void Install(IServiceCollection services, IConfiguration configuration)
        {
            // Add the domain events to outbox messages interceptor as a singleton
            services.AddSingleton<ConvertDomainEventsToOutboxMessagesInterceptor>();

            // Configure the application's DbContext to use SQL Server with the
            // provided connection string
            services.AddDbContext<ApplicationDbContext>(
                options => options.UseNpgsql(
                    configuration.GetConnectionString("Database")));
        }
    }
}