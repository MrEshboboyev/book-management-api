using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace BookManagement.Infrastructure.Authentication;

/// <summary>
/// Custom authorization policy provider to handle dynamic permission-based policies.
/// </summary>
public class PermissionAuthorizationPolicyProvider(
    IOptions<AuthorizationOptions> options)
    : DefaultAuthorizationPolicyProvider(options)
{
    /// <summary>
    /// Retrieves the authorization policy based on the provided policy name.
    /// </summary>
    /// <param name="policyName">The name of the policy to retrieve.</param>
    /// <returns>A task that represents the asynchronous operation, containing the authorization policy.</returns>
    public override async Task<AuthorizationPolicy> GetPolicyAsync(string policyName)
    {
        #region Get Policy

        // Attempt to retrieve the policy from the base provider
        var policy = await base.GetPolicyAsync(policyName);
        if (policy is not null)
        {
            return policy;
        }

        #endregion

        // Create a new policy with the required permission if not found in the base provider
        return new AuthorizationPolicyBuilder()
            .AddRequirements(new PermissionRequirement(policyName))
            .Build();
    }
}