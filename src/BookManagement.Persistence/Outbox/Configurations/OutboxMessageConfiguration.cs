using BookManagement.Persistence.Outbox.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Persistence.Outbox.Configurations;

/// <summary> 
/// Configures the OutboxMessage entity for Entity Framework Core. 
/// </summary>
internal sealed class OutboxMessageConfiguration : IEntityTypeConfiguration<OutboxMessage>
{
    public void Configure(EntityTypeBuilder<OutboxMessage> builder)
    {
        // Map to the OutboxMessages table
        builder.ToTable(OutboxTableNames.OutboxMessages);

        // Configure the primary key
        builder.HasKey(x => x.Id);
    }
}
