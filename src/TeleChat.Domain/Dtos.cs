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

public class ReactionDto
{
    public int MessageReactionId { get; set; }
    public int ReactionId { get; set; }
    public string Value { get; set; } = string.Empty;
    public int UserId { get; set; }
    public int MessageId { get; set; }
    public string? ConnectionId { get; set; }
}