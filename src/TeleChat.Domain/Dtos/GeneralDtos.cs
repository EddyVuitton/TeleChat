namespace TeleChat.Domain.Dtos;

public record MessageDto(
	string Text,
    string ConnectionId,
    int MessageTypeId,
    int UserId,
    int GroupChatId
);