using BookManagement.Domain.Entities.Users;
using BookManagement.Persistence.Users.Constants;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace BookManagement.Persistence.Users.Configurations.Roles;

/// <summary> 
/// Configures the Role entity for Entity Framework Core. 
/// </summary>
internal sealed class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> builder)
    {
        // Map to the Roles table
        builder.ToTable(UserTableNames.Roles);

        // Configure the primary key
        builder.HasKey(x => x.Id);

        // Configure many-to-many relationships
        builder.HasMany(x => x.Permissions)
            .WithMany()
            .UsingEntity<RolePermission>();
        
        builder.HasMany(x => x.Users)
            .WithMany(x => x.Roles);

        // Seed initial data
        builder.HasData(Role.GetValues());
    }
}
