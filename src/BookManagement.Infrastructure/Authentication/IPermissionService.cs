namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Service interface for managing user permissions.
/// </summary>
public interface IPermissionService
{
    /// <summary>
    /// Retrieves the set of permissions assigned to a specific user.
    /// </summary>
    /// <param name="userId">The unique identifier of the user.</param>
    /// <returns>A task that represents the asynchronous operation,
    /// containing a hash set of permission strings.</returns>
    Task<HashSet<string>> GetPermissionsAsync(Guid userId);
}