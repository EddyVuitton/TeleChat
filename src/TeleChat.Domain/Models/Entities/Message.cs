namespace TeleChat.Domain.Models.Entities;

public class Message
{
    public int Id { get; set; }
    public string Text { get; set; } = null!;
    public int MessageTypeId { get; set; }
    public int UserId { get; set; }
    public int GroupChatId { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public virtual MessageType? MessageType { get; set; }
    public virtual User? User { get; set; }
    public virtual GroupChat? GroupChat { get; set; }
}