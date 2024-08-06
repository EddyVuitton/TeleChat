using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TeleChat.Domain.Auth;

namespace TeleChat.WebAPI.Helpers;

public static class AuthHelper
{
    public static string HashPassword(string password)
    {
        var passwordBytes = Encoding.Default.GetBytes(password ?? string.Empty);
        var hashedPassword = SHA256.HashData(passwordBytes);

        return Convert.ToHexString(hashedPassword);
    }

    public static UserToken BuildToken(List<Claim>? claims, string? issuer, string? audience, string jwtKey)
    {
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expiration = DateTime.Now.AddDays(1);

        JwtSecurityToken token = new(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: expiration,
            signingCredentials: creds
        );

        return new UserToken()
        {
            Token =  new JwtSecurityTokenHandler().WriteToken(token)
        };
    }
}