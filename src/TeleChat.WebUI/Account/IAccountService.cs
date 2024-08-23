using TeleChat.Domain.Auth;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain.Forms;

namespace TeleChat.WebUI.Account;

public interface IAccountService
{
    Task<UserToken> LoginAsync(LoginAccountForm form);
    Task RegisterAsync(RegisterAccountForm form);
    Task<User?> GetUserByLoginAsync(string login);
    Task<UserToken> GetTokenAsync();
    Task<User?> CreateUser(string name);
}