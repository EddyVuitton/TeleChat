namespace TeleChat.Domain;

public record MessageDto(
    string Text,
    string ConnectionId,
    int MessageTypeId,
    int UserId,
    int GroupChatId,
    string UserName
);