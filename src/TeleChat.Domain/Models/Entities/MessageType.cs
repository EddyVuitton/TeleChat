namespace TeleChat.Domain.Models.Entities;

public class MessageType
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? DefaultStyle { get; set; }
}