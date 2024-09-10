using TeleChat.Domain.Models.Entities;

namespace TeleChat.Domain;

public record MessageDto(
    string Text,
    string ConnectionId,
    int MessageTypeId,
    int UserId,
    int GroupChatId,
    string UserName
);

public record GroupChatDto(
    string Name,
    User User,
    List<User>? Members
);