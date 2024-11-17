namespace TeleChat.Domain.Models.Entities;

public class MessageReaction
{
    public int Id { get; set; }
    public int MessageId { get; set; }
    public int ReactionId { get; set; }
    public int UserId { get; set; }

    public virtual Message? Message { get; set; }
    public virtual Reaction? Reaction { get; set; }
    public virtual User? User { get; set; }
}