using TeleChat.Domain.Dtos;

namespace TeleChat.WebUI.Services.Main;

public interface IMainService
{
    Task AddToGroupAsync(string connectionId, string groupName);
    Task SendToGroupAsync(string connectionId, string userName, string message, string groupName);
    Task SendMessageAsync(MessageDto message);
}