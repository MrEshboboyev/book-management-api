using BookManagement.Application.Abstractions.Security;
using BookManagement.Domain.Entities.Users;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace BookManagement.Infrastructure.Authentication;

internal sealed class JwtProvider(IOptions<JwtOptions> options,
    IPermissionService permissionService) : IJwtProvider
{
    private readonly JwtOptions _options = options.Value;
    private readonly IPermissionService _permissionService = permissionService;

    public async Task<string> GenerateAsync(User user)
    {
        var claims = new List<Claim>
        {
                new(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new(JwtRegisteredClaimNames.Email, user.Email.Value)
        };

        HashSet<string> permissions = await _permissionService
               .GetPermissionsAsync(user.Id);

        foreach (string permission in permissions)
        {
            claims.Add(new(CustomClaims.Permissions, permission));
        }

        var signingCredentials = new SigningCredentials(
             new SymmetricSecurityKey(
                 Encoding.UTF8.GetBytes(_options.SecretKey)),
             SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            _options.Issuer,
            _options.Audience,
            claims,
            null,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        string tokenValue = new JwtSecurityTokenHandler()
             .WriteToken(token);

        return tokenValue;
    }
}