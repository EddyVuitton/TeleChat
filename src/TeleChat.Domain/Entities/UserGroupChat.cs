using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TeleChat.Domain.Entities;

public class UserGroupChat
{
    [Key]
    public int Id { get; set; }
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    [ForeignKey(nameof(GroupChat))]
    public int GroupChatId { get; set; }
    [Required]
    public DateTime Created { get; set; } = DateTime.UtcNow;

    public virtual User User { get; set; } = new();
    public virtual GroupChat GroupChat { get; set; } = new();
}