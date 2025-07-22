using BookManagement.Domain.Primitives;
using BookManagement.Persistence.Outbox;
using BookManagement.Persistence;
using MediatR;
using Newtonsoft.Json;
using Quartz;
using Microsoft.EntityFrameworkCore;
using Polly;

namespace BookManagement.Infrastructure.BackgroundJobs;

/// <summary> 
/// Background job for processing outbox messages. This job retrieves unprocessed outbox messages, 
/// deserializes the domain events, publishes them, and marks the messages as processed. 
/// </summary>
/// <remarks> 
/// Initializes a new instance of the <see cref="ProcessOutboxMessagesJob"/> class. 
/// </remarks> 
/// <param name="dbContext">The application database context.</param> 
/// <param name="publisher">The publisher for domain events.</param>
[DisallowConcurrentExecution]
public class ProcessOutboxMessagesJob(ApplicationDbContext dbContext, IPublisher publisher) : IJob
{
    /// <summary> 
    /// Executes the job to process outbox messages. 
    /// </summary> 
    /// <param name="context">The job execution context.</param>
    public async Task Execute(IJobExecutionContext context)
    {
        #region Get unprocessed messages
        
        // Retrieve unprocessed outbox messages
        var messages = await dbContext
            .Set<OutboxMessage>()
            .Where(m => m.ProcessedOnUtc == null)
            .Take(20)
            .ToListAsync(context.CancellationToken);
        
        #endregion

        #region Process outbox messages
        
        // Process each outbox message
        var policy = Policy
            .Handle<Exception>()
            .WaitAndRetryAsync(3, 
                attempt => TimeSpan.FromMilliseconds(50 * attempt));
        

        foreach (var outboxMessage in messages)
        {
            // Deserialize the domain event from the message content
            var domainEvent = JsonConvert.DeserializeObject<IDomainEvent>(
                outboxMessage.Content,
                new JsonSerializerSettings
                {
                    TypeNameHandling = TypeNameHandling.All
                });

            if (domainEvent is null)
            {
                continue;
            }

            // Execute the publish operation with retry policy
            var result = await policy.ExecuteAndCaptureAsync(() =>
                publisher.Publish(domainEvent, context.CancellationToken));

            // Record any errors that occurred during publishing
            outboxMessage.Error = result.FinalException?.ToString();

            // Mark the outbox message as processed
            outboxMessage.ProcessedOnUtc = DateTime.UtcNow;
        }
        
        #endregion

        // Save changes to the database
        await dbContext.SaveChangesAsync();
    }
}
