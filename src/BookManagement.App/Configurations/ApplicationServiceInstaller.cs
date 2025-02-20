using FluentValidation;
using BookManagement.Application;
using BookManagement.Application.Behaviors;
using BookManagement.Infrastructure.Idempotence;
using MediatR;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs application services and configuration.
/// </summary>
public class ApplicationServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the application services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Add MediatR services for handling commands and queries
        services.AddMediatR(cfg =>
        {
            // Register handlers from the specified assembly
            cfg.RegisterServicesFromAssembly(AssemblyReference.Assembly);
        });

        // Add validation behavior to the pipeline
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(ValidationPipelineBehavior<,>));

        // Add logging behavior to the pipeline
        services.AddScoped(typeof(IPipelineBehavior<,>), typeof(LoggingPipelineBehavior<,>));

        // Decorate notification handlers with idempotent behavior
        services.Decorate(typeof(INotificationHandler<>), typeof(IdempotentDomainEventHandler<>));

        // Add FluentValidation validators from the specified assembly
        services.AddValidatorsFromAssembly(
            AssemblyReference.Assembly,
            includeInternalTypes: true);
    }
}