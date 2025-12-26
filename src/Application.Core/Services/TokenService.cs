using Application.Domain.Entities;
using Application.Domain.Interfaces.Services;
using Application.Domain.Util;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;

namespace Application.Core.Services;

public class TokenService(IConfiguration configuration, UserManager<User> userManager) : ITokenService
{
    public string GenerateRefreshToken()
    {
        byte[] randomBytes = RandomNumberGenerator.GetBytes(64);
        return Convert.ToBase64String(randomBytes);
    }

    public async Task<string> GerarToken(User user)
    {
        DateTime expiresAt = DateTime.UtcNow.AddMinutes(10);
        List<Claim> claims =
        [
            new(ClaimTypes.Name, user.UserName!),
            new(ClaimTypes.Email, user.Email!),
            new(ClaimTypes.NameIdentifier, user.Id)
        ];

        IList<string> roles = await userManager.GetRolesAsync(user);

        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        SecurityTokenDescriptor tokenDescriptor = new()
        {
            Subject = new ClaimsIdentity(claims),
            Expires = expiresAt,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(ClsGlobal.GetTokenKey(configuration)),
                SecurityAlgorithms.HmacSha256Signature
            ),
            IssuedAt = expiresAt
        };

        JwtSecurityTokenHandler tokenHandler = new();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
        return tokenHandler.WriteToken(token);
    }
}
