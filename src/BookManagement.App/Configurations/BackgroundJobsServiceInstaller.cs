using BookManagement.Infrastructure.BackgroundJobs;
using Quartz;

namespace BookManagement.App.Configurations;

/// <summary>
/// Installs background job services and configuration.
/// </summary>
public class BackgroundJobsServiceInstaller : IServiceInstaller
{
    /// <summary>
    /// Configures the background job services.
    /// </summary>
    /// <param name="services">The collection of services to configure.</param>
    /// <param name="configuration">The application configuration.</param>
    public void Install(IServiceCollection services, IConfiguration configuration)
    {
        // Register the job with DI
        services.AddScoped<IJob, ProcessOutboxMessagesJob>();

        // Configure Quartz
        services.AddQuartz(configure =>
        {
            var jobKey = new JobKey(nameof(ProcessOutboxMessagesJob));
            configure
                .AddJob<ProcessOutboxMessagesJob>(jobKey) // Add the job
                .AddTrigger( // Add a trigger
                    trigger =>
                        trigger.ForJob(jobKey)
                            .WithSimpleSchedule(
                                schedule =>
                                    schedule.WithIntervalInSeconds(10)
                                        .RepeatForever()));
        });

        // Add Quartz as a hosted service
        services.AddQuartzHostedService();
    }
}