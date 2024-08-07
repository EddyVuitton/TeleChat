using TeleChat.Domain.Entities;

namespace TeleChat.Domain.Dtos;

public record UserMessageDto(int Id, string From, MessageDto Message);
public record MessageDto(
    string ConnectionId,
    string GroupName,
    User User,
    Message Message
);