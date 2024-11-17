using Microsoft.AspNetCore.SignalR.Client;
using TeleChat.Domain.Models.Entities;
using TeleChat.Domain;

namespace TeleChat.WebUI.Services.App;

public interface IAppService
{
    HubConnection CreateHubConnection(string token);
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
    Task<Message?> SendMessageAsync(MessageDto message);
    Task<GroupChat?> GetDefaultGroupChatAsync();
    Task<List<Message>> GetGroupChatMessagesAsync(int groupChatId);
    Task<UserGroupChat?> AddGroupChatAsync(GroupChatDto groupChat);
    Task DeleteGroupChatAsync(int groupChatId);
    Task DeleteMessageAsync(int messageId);
    Task<List<Reaction>> GetReactionsAsync();
    Task<MessageReaction?> AddReactionAsync(ReactionDto reaction);
    Task<List<ReactionDto>> GetChatReactionsAsync(int chatId);
}