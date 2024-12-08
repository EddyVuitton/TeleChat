using TeleChat.Domain;
using TeleChat.Domain.Models.Entities;

namespace TeleChat.WebAPI.Repositories.App;

public interface IAppRepository
{
    Task AddConnectionToGroupAsync(string connectionId, Guid groupChatGuid);
    Task<Message> SendMessageAsync(MessageDto message);
    Task<List<MessageType>> GetMessageTypesAsync();
    Task<List<UserGroupChat>> GetUserGroupChatsAsync(int userId);
    Task<GroupChat?> GetDefaultGroupChatAsync();
    Task<List<Message>> GetGroupChatMessagesAsync(int groupChatId);
    Task<UserGroupChat> AddGroupChatAsync(GroupChatDto groupChat);
    Task DeleteGroupChatAsync(int groupChatId);
    Task DeleteMessageAsync(int messageId);
    Task<List<Reaction>> GetReactionsAsync();
    Task<MessageReaction> AddReactionAsync(ReactionDto dto);
    Task<List<ReactionDto>> GetChatReactionsAsync(int chatId);
    Task RemoveReactionAsync(ReactionDto dto);
}