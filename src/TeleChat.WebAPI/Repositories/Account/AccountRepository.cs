using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using TeleChat.Domain.Auth;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;
using TeleChat.WebAPI.Helpers;
using TeleChat.WebAPI.Options.JWT;
using TeleChat.Domain;

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
            Password = AuthHelper.HashPassword(form.Password)
        };

        await _context.User.AddAsync(newUser);
        await _context.SaveChangesAsync();
    }

    public async Task<User?> GetUserByLoginAsync(string login)
    {
        return await _context.User.AsNoTracking().FirstOrDefaultAsync(x => x.Login == login);
    }

    public UserToken GetToken(string issuer, string audience)
    {
        var jtwKey = _jwtOptions.Value.Key;
        var claims = new List<Claim>();

        var token = AuthHelper.BuildToken(claims, issuer, audience, jtwKey);

        return token;
    }

    public async Task<User?> CreateUser(string name)
    {
        var checkIfUserExists = await _context.User.FirstOrDefaultAsync(x => x.Name == name);

        if (checkIfUserExists is not null)
        {
            return checkIfUserExists;
        }

        var newUser = new User()
        {
            Name = name,
            Login = name,
            Password = AuthHelper.HashPassword(name)
        };

        await _context.AddAsync(newUser);
        await _context.SaveChangesAsync();

        return newUser;
    }

    #endregion
}