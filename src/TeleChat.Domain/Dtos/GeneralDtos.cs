namespace TeleChat.Domain.Dtos;

public record UserMessageDto (int Id, string From, MessageDto Message);
public record MessageDto(string MessageValue, DateTime MessageDate);