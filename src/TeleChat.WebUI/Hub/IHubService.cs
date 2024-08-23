using Microsoft.AspNetCore.SignalR.Client;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain;

namespace TeleChat.WebUI.Hub;

public interface IHubService
{
    HubConnection CreateHubConnection(string token);
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
    Task<Message?> SendMessageAsync(MessageDto message);
    Task<GroupChat> GetDefaultGroupChatAsync();
}