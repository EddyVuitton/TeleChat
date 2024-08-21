using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TeleChat.Domain.Auth;

namespace TeleChat.WebAPI.Helpers;

public static class AuthHelper
{
    private const int _SaltSize = 16;
    private const int _HashSize = 16;
    private const int _Iterations = 100_000;
    private const char _PasswordSeperator = '-';

    private static readonly HashAlgorithmName _algorithm = HashAlgorithmName.SHA512;

    public static string HashPassword(string password)
    {
        var salt = RandomNumberGenerator.GetBytes(_SaltSize);
        var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _Iterations, _algorithm, _HashSize);
        var hexSalt = Convert.ToHexString(salt);
        var hexHash = Convert.ToHexString(hash);

        return $"{hexHash}{_PasswordSeperator}{hexSalt}";
    }

    public static bool Verify(string password, string passwordHash)
    {
        var parts = passwordHash.Split(_PasswordSeperator);
        var hash = Convert.FromHexString(parts[0]);
        var salt = Convert.FromHexString(parts[1]);

        var inputHash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _Iterations, _algorithm, _HashSize);

        return CryptographicOperations.FixedTimeEquals(hash, inputHash);
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