using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Context;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;
using TeleChat.WebAPI.Helpers;
using TeleChat.WebAPI.Options.JWT;

namespace TeleChat.WebAPI.Repositories.Account;

public class AccountRepository(DBContext context, IOptions<JWTOptions> options) : IAccountRepository
{
    private readonly DBContext _context = context;
    private readonly IOptions<JWTOptions> _jwtOptions = options;

    #region Publics

    public async Task<UserToken> LoginAsync(string login, string password, string issuer, string audience)
    {
        var user = await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login)
            ?? throw new Exception("Nie znaleziono użytkownika o podanym loginie");

        var verified = AuthHelper.Verify(password, user.Password);

        if (verified)
        {
            var jtwKey = _jwtOptions.Value.Key;
            var claims = new List<Claim>()
            {
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

    public async Task RegisterAsync(RegisterAccountForm form)
    {
        form = form ?? throw new ArgumentNullException(nameof(form));

        var checkIfUserExists = await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Login == form.Login);

        if (checkIfUserExists is not null)
        {
            throw new Exception("Użytkownik o podanym loginie już istnieje");
        }

        var newUser = new User()
        {
            Login = form.Login,
            Name = form.Name,
            Password = AuthHelper.HashPassword(form.Password),
            IsActive = true
        };

        await _context.User.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByLoginAsync(string login) => await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login);

    #endregion
}