using TeleChat.Domain.Auth;
using TeleChat.Domain.Entities;
using TeleChat.Domain.Forms;

namespace TeleChat.WebUI.Services.Account;

public interface IAccountService
{
    Task<UserToken> LoginAsync(LoginAccountForm form);
    Task RegisterAsync(RegisterAccountForm form);
    Task<User?> GetUserByLoginAsync(string login);
}