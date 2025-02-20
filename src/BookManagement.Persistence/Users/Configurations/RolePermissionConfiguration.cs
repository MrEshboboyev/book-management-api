using BookManagement.Domain.Enums.Users;
using BookManagement.Domain.Entities.Users;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = BookManagement.Domain.Enums.Users.Permission;

namespace BookManagement.Persistence.Users.Configurations;

/// <summary>
/// Configures the RolePermission entity for Entity Framework Core.
/// </summary>
internal sealed class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Configure the composite primary key
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        // Seed initial data
        builder.HasData(
            Create(Role.Registered, Permission.ReadUser),
            Create(Role.Registered, Permission.UpdateUser),
            Create(Role.Registered, Permission.AddBook),
            Create(Role.Registered, Permission.AddBooksBulk),
            Create(Role.Registered, Permission.UpdateBook),
            Create(Role.Registered, Permission.SoftDeleteBook),
            Create(Role.Registered, Permission.SoftDeleteBooksBulk),
            Create(Role.Registered, Permission.GetBookDetails),
            Create(Role.Registered, Permission.GetBooksList)
        );
    }

    private static RolePermission Create(Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
