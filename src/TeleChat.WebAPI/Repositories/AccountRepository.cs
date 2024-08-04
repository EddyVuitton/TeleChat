using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Context;
using TeleChat.Domain.Entities;
using TeleChat.WebAPI.Helpers;
using TeleChat.WebAPI.Options.JWT;

namespace TeleChat.WebAPI.Repositories;

public class AccountRepository(DBContext context, IOptions<JWTOptions> options)
{
    private readonly DBContext _context = context;
    private readonly IOptions<JWTOptions> _jwtOptions = options;

    #region Publics

    public async Task<UserToken> LoginAsync(string login, string password, string issuer, string audience)
    {
        {
            await _context.User.AddAsync(new User()
            {
                Id = 1,
                Login = login,
                Name = "Eddy",
                Password = AuthHelper.HashPassword(password)
            });
            await _context.SaveChangesAsync();
        }

        var user = await _context.User.FirstOrDefaultAsync(x => x.Login == login);

        if (user is null)
        {
            throw new Exception("Nie znaleziono użytkownika o podanym loginem...");
        }

        var hashedPassword = AuthHelper.HashPassword(password);

        var result = user.Password == hashedPassword;

        if (result)
        {
            var jtwKey = _jwtOptions.Value.Key;
            var claims = new List<Claim>()
            {
                new(ClaimTypes.NameIdentifier, user.Name),
                new("UserLogin", user.Login)
            };

            var token = AuthHelper.BuildToken(claims, issuer, audience, jtwKey);

            return token;
        }
        else
        {
            throw new Exception("Nieprawidłowy login lub hasło");
        }
    }

    #endregion

    #region Privates


    #endregion
}