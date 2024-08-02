using TeleChat.Domain.Auth;

namespace TeleChat.WebUI.Services.Main;

public interface IMainService
{
    Task<UserToken> BuildTokenAsync(string userName);
    Task AddToGroupAsync(string connectionId, string groupName);
    Task SendToGroupAsync(string connectionId, string userName, string message, string groupName);
}