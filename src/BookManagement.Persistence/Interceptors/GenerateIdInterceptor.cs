using BookManagement.Domain.Primitives;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace BookManagement.Persistence.Interceptors;

public sealed class GenerateIdInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        var context = eventData.Context;
        if (context is null)
            return base.SavingChangesAsync(eventData, result, cancellationToken);

        foreach (var entry in context.ChangeTracker.Entries<Entity>())
        {
            if (entry.State == EntityState.Added)
            {
                var idProperty = entry.Property(nameof(Entity.Id));

                if (idProperty.Metadata.ClrType == typeof(Guid) &&
                    (idProperty.CurrentValue == null || (Guid)idProperty.CurrentValue == Guid.Empty))
                {
                    idProperty.CurrentValue = Guid.CreateVersion7();
                }
            }
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}
