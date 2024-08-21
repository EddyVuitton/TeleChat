using TeleChat.Domain.Dtos;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebUI.Services.Main;

public interface IMainService
{
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
    Task<Message?> SendMessageAsync(MessageDto message);
}