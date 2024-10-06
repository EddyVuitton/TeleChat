namespace TeleChat.Domain.Models.Entities;

public class UserGroupChat
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int GroupChatId { get; set; }
    public DateTime Created { get; set; } = DateTime.Now;

    public virtual User? User { get; set; }
    public virtual GroupChat? GroupChat { get; set; }
}