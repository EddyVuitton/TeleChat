using TeleChat.Domain.Auth;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;

namespace TeleChat.WebAPI.Repositories.Account;

public interface IAccountRepository
{
    Task<UserToken> LoginAsync(string login, string password, string issuer, string audience);
    Task RegisterAsync(RegisterAccountForm form);
    Task<User?> GetUserByLoginAsync(string login);
    UserToken GetToken(string issuer, string audience);
    Task<User?> CreateUser(string name);
}