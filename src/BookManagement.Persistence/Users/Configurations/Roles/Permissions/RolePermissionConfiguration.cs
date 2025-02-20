using BookManagement.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Permission = BookManagement.Domain.Enums.Permission;

namespace BookManagement.Persistence.Users.Configurations.Roles.Permissions;

/// <summary> 
/// Configures the RolePermission entity for Entity Framework Core. 
/// </summary>
internal sealed class RolePermissionConfiguration
    : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> builder)
    {
        // Configure the composite primary key
        builder.HasKey(x => new { x.RoleId, x.PermissionId });

        // Seed initial data
        builder.HasData(
            Create(Role.Registered, Permission.ReadUser),
            Create(Role.Registered, Permission.UpdateUser));
    }

    private static RolePermission Create(
        Role role, Permission permission)
    {
        return new RolePermission
        {
            RoleId = role.Id,
            PermissionId = (int)permission
        };
    }
}
