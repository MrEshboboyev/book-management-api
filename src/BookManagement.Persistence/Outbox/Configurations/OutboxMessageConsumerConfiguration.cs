using BookManagement.Persistence.Outbox.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Persistence.Outbox.Configurations;

/// <summary> 
/// Configures the OutboxMessageConsumer entity for Entity Framework Core. 
/// </summary>
internal sealed class OutboxMessageConsumerConfiguration : 
    IEntityTypeConfiguration<OutboxMessageConsumer>
{
    public void Configure(EntityTypeBuilder<OutboxMessageConsumer> builder)
    {
        // Map to the OutboxMessageConsumers table
        builder.ToTable(OutboxTableNames.OutboxMessageConsumers);

        // Configure the composite primary key
        builder.HasKey(outboxMessageConsumer => new
        {
            outboxMessageConsumer.Id,
            outboxMessageConsumer.Name
        });
    }
}
