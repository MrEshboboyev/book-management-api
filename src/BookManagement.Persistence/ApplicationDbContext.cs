using BookManagement.Application.Common.Data;
using BookManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Persistence;

/// <summary> 
/// Represents the database context for the application. 
/// This context manages the connection to the database and provides methods for querying and saving data. 
/// </summary>
/// <remarks> 
/// Initializes a new instance of the <see cref="ApplicationDbContext"/> class with the specified options. 
/// </remarks> 
/// <param name="options">The options to be used by the DbContext.</param>
public sealed class ApplicationDbContext(
    DbContextOptions options
) : DbContext(options), IApplicationDbContext
{
    // Explicit DbSet properties for direct access
    public DbSet<User> Users => Set<User>();

    /// <summary> 
    /// Configures the model that was discovered by convention from the entity types 
    /// exposed in <see cref="DbSet{TEntity}"/> properties on your derived context. 
    /// </summary> 
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder) =>
              modelBuilder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);
}
