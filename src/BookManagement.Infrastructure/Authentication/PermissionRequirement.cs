using Microsoft.AspNetCore.Authorization;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Represents an authorization requirement based on a specific permission.
/// </summary>
/// <param name="permission">The permission required to access the resource.</param>
public class PermissionRequirement(string permission) : IAuthorizationRequirement
{
    /// <summary>
    /// Gets the required permission.
    /// </summary>
    public string Permission { get; } = permission;
}