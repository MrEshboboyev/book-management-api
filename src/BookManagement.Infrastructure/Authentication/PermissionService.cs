using BookManagement.Domain.Entities.Users;
using BookManagement.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Service for managing user permissions.
/// </summary>
public class PermissionService(ApplicationDbContext context) : IPermissionService
{
    /// <summary>
    /// Retrieves the set of permissions assigned to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation,
    /// containing a hash set of permission strings.</returns>
    public async Task<HashSet<string>> GetPermissionsAsync(Guid userId)
    {
        // Get the roles and their permissions for the specified user
        var roles = await context.Set<User>()
            .Include(x => x.Roles)
            .ThenInclude(x => x.Permissions)
            .Where(x => x.Id == userId)
            .Select(x => x.Roles)
            .ToArrayAsync();

        // Extract and return the permission names as a hash set
        return roles
            .SelectMany(x => x)
            .SelectMany(x => x.Permissions)
            .Select(x => x.Name)
            .ToHashSet();
    }
}