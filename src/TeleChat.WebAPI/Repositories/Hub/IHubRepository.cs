using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebAPI.Repositories.Hub;

public interface IHubRepository
{
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task<Message> SendMessageAsync(MessageDto message);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
}