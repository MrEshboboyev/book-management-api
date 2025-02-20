using Microsoft.AspNetCore.Authorization;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Handles authorization requirements for checking user permissions.
/// </summary>
public class PermissionAuthorizationHandler
    : AuthorizationHandler<PermissionRequirement>
{
    /// <summary>
    /// Handles the requirement to check if the user has the specified permission.
    /// </summary>
    /// <param name="context">The authorization handler context.</param>
    /// <param name="requirement">The permission requirement to be checked.</param>
    /// <returns>A completed task representing the authorization result.</returns>
    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirement requirement)
    {
        #region Get Permissions
        
        // Retrieve the permissions claims from the user
        var permissions = context
            .User
            .Claims
            .Where(x => x.Type == CustomClaims.Permissions)
            .Select(x => x.Value)
            .ToHashSet();
        
        #endregion
        
        #region Check permissions contains

        // Check if the user's permissions contain the required permission
        if (permissions.Contains(requirement.Permission))
        {
            context.Succeed(requirement);
        }
        
        #endregion

        // Return a completed task
        return Task.CompletedTask;
    }
}