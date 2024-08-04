using TeleChat.Domain.Auth;
using TeleChat.Domain.Forms;

namespace TeleChat.WebUI.Services.Main;

public interface IMainService
{
    Task<UserToken> LoginAsync(LoginAccountForm form);
    Task AddToGroupAsync(string connectionId, string groupName);
    Task SendToGroupAsync(string connectionId, string userName, string message, string groupName);
}