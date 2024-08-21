using TeleChat.Domain.Dtos;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebAPI.Repositories.Main;

public interface IMainRepository
{
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task SendToGroupAsync(string connectionId, Message message);
    Task<Message> SendMessageAsync(MessageDto message);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
}